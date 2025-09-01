using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanzApp.core.Interfaces;
using FinanzApp.core.Models;
using FinanzApp.Interfaces;
using FinanzApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;


namespace FinanzApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        #region Constructor

        public MainViewModel(ITransactionService transactionService, IDialogService dialogService, INotificationService notificationService)
        {
            _dialogService = dialogService;
            _transactionService = transactionService;
            _notification = notificationService;
            Transactions = new ObservableCollection<Transaction>();

            var cvsIncome = new CollectionViewSource { Source = Transactions };
            IncomesView = cvsIncome.View;
            IncomesView.Filter = o =>((Transaction)o).TransactionType == TransactionType.Income;
            cvsIncome.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Transaction.Category)));


            var cvsExpense = new CollectionViewSource { Source = Transactions };
            ExpensesView = cvsExpense.View;
            ExpensesView.Filter = o => ((Transaction)o).TransactionType == TransactionType.Expense;
            cvsExpense.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Transaction.Category)));


            _ = RefreshTransactionsAsync();
            //RefreshTransactionsCommand.Execute(null);
        }

        #endregion

        #region Fields

        private readonly IDialogService _dialogService;
        private readonly ITransactionService _transactionService;
        private readonly INotificationService _notification;

        [ObservableProperty]
        private Transaction? _selectedIncome;

        [ObservableProperty]
        private Transaction? _selectedExpense;

        #endregion

        #region Properties
        public ObservableCollection<Transaction> Transactions { get; set; }

        public IEnumerable<Transaction> Incomes => Transactions.Where
            (t => t.TransactionType == TransactionType.Income);

        public IEnumerable<Transaction> Expenses => Transactions.Where
            (t => t.TransactionType == TransactionType.Expense);

        public decimal IncomeSum => Transactions?
                .Where(t => t.TransactionType == TransactionType.Income)
                .Sum(t => t.Amount) ?? 0;

        public decimal ExpenseSum => (Transactions?
            .Where(t => t.TransactionType == TransactionType.Expense)
            .Sum(t => t.Amount)) ?? 0;

        public decimal BilanceSum => IncomeSum - ExpenseSum;

        public ICollectionView IncomesView { get; }
        public ICollectionView ExpensesView { get; }

        #endregion

        #region Methods

        [RelayCommand]
        private async Task AddTransactionAsync(TransactionType transactionType)
        {
            var result = _dialogService.ShowTransactionDialog(null, transactionType);
            if (result != null)
            {
                try
                {
                    await _transactionService.AddAsync(result);
                    await RefreshTransactionsAsync();
                }
                catch (Exception ex)
                {
                    var fullmessage = ex.InnerException?.Message ?? ex.Message;
                    _notification.Error($"Fehler beim hinzufügen der Transaktion: {fullmessage}");
                    _notification.Error($"SQL-Fehler:\n{ex.GetBaseException().Message}");
                    Debug.WriteLine(ex.ToString()); // für vollständige Trace in der Ausgabe
                }
            }
        }

        [RelayCommand(CanExecute = nameof(CanExecuteTransactionCommands))]
        private async Task DeleteTransaction(Transaction transaction)
        {
            try
            {
                if (transaction != null)
                {
                    await _transactionService.DeleteAsync(transaction);
                    await RefreshTransactionsAsync();
                }
            }
            catch (Exception ex)
            {
                var fullmessage = ex.InnerException?.Message ?? ex.Message;
                _notification.Error($"Fehler beim löschen der Transaktion: {fullmessage}");
            }
        }

        [RelayCommand(CanExecute = nameof(CanExecuteTransactionCommands))]
        private async Task EditTransaction(Transaction transaction)
        {
            if (transaction is null)
                return;

            var copyTransaction = CloneTransaction(transaction);
            var result = _dialogService.ShowTransactionDialog(copyTransaction);

            if (result != null)
            {
                transaction.Name = result.Name;
                transaction.Description = result.Description;
                transaction.Date = result.Date;
                transaction.Category = result.Category;
                transaction.Amount = result.Amount;
                transaction.BudgetAmount = result.BudgetAmount;
                transaction.TransactionType = result.TransactionType;
                try
                {
                    await _transactionService.UpdateAsync(transaction);
                    await RefreshTransactionsAsync();
                }
                catch (Exception ex)
                {
                    var fullmessage = ex.InnerException?.Message ?? ex.Message;
                    _notification.Error($"Fehler beim Aktualisieren der Transaktion: {fullmessage}");
                }
            }
        }

        private bool CanExecuteTransactionCommands(Transaction transaction)
        {
            return transaction != null;
        }


        [RelayCommand]
        private async Task RefreshTransactionsAsync()
        {
            Transactions.Clear();
            try
            {
                var loadedTransactions = await _transactionService.GetAllAsync();

                foreach (var transaction in loadedTransactions)
                    Transactions.Add(transaction);

                OnPropertyChanged(nameof(Expenses));
                OnPropertyChanged(nameof(Incomes));
                OnPropertyChanged(nameof(IncomeSum));
                OnPropertyChanged(nameof(ExpenseSum));
                OnPropertyChanged(nameof(BilanceSum));
            }
            catch (Exception ex)
            {
                var fullmessage = ex.InnerException?.Message ?? ex.Message;
                _notification.Error($"Fehler beim Laden der Transaktion: {fullmessage}");
            }

            IncomesView.Refresh();
            ExpensesView.Refresh();
        }

        private Transaction CloneTransaction(Transaction originalTransaction)
        {
            return new Transaction
            {
                Name = originalTransaction.Name,
                Description = originalTransaction.Description,
                Date = originalTransaction.Date,
                Category = originalTransaction.Category,
                Amount = originalTransaction.Amount,
                BudgetAmount = originalTransaction.BudgetAmount,
                TransactionType = originalTransaction.TransactionType,
            };
        }

        #endregion
    }
}
