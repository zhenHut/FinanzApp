using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanzApp.Converters;
using FinanzApp.core.Interfaces;
using FinanzApp.core.Models;
using FinanzApp.Interfaces;
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

            var cvsIncome = new CollectionViewSource { Source = Incomes };
            IncomesView = cvsIncome.View;
            cvsIncome.GroupDescriptions.Add(new PropertyGroupDescription("Category.Name", new NullToLabelConverter()));


            var cvsExpense = new CollectionViewSource { Source = Expenses };
            ExpensesView = cvsExpense.View;
            cvsExpense.GroupDescriptions.Add(new PropertyGroupDescription("Category.Name", new NullToLabelConverter()));


            _ = RefreshTransactionsAsync();

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
        public ObservableCollection<Transaction> Transactions { get; } = new();
        public ObservableCollection<Transaction> Incomes { get; } = new();
        public ObservableCollection<Transaction> Expenses { get; } = new();

        public ICollectionView IncomesView { get; }
        public ICollectionView ExpensesView { get; }


        public decimal IncomeSum => Incomes.Sum(t => t.Amount);
        public decimal ExpenseSum => Expenses.Sum(t => t.Amount);


        public decimal BilanceSum => IncomeSum - ExpenseSum;

        #endregion

        #region Methods

        [RelayCommand]
        private async Task AddTransactionAsync(TransactionType transactionType)
        {
            var result = _dialogService.ShowTransactionDialog(null, transactionType);
            if (result is null)
                return;

            try
            {
                await _transactionService.AddAsync(result);
                Transactions.Add(result);

                if (result.TransactionType == TransactionType.Income)
                    Incomes.Add(result);

                if (result.TransactionType == TransactionType.Expense)
                    Expenses.Add(result);

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

            if (result is null)
                return;

                try
                {
                    //var oldType = transaction.TransactionType;
                    transaction.Name = result.Name;
                    transaction.Description = result.Description;
                    transaction.Date = result.Date;
                    transaction.Category = null;
                    transaction.CategoryId = result.CategoryId;
                    transaction.Amount = result.Amount;
                    transaction.BudgetAmount = result.BudgetAmount;
                    transaction.TransactionType = result.TransactionType;


                    await _transactionService.UpdateAsync(transaction);

                //if (oldType == TransactionType.Income)
                //    Incomes.Remove(transaction);

                //if(oldType == TransactionType.Expense)
                //    Expenses.Remove(transaction);

                //if (transaction.TransactionType == TransactionType.Income) 
                //    Incomes.Add(transaction);

                //if (transaction.TransactionType == TransactionType.Expense)
                //    Expenses.Add(transaction);

                await RefreshTransactionsAsync();
                }
                catch (Exception ex)
                {
                    var fullmessage = ex.InnerException?.Message ?? ex.Message;
                    _notification.Error($"Fehler beim Aktualisieren der Transaktion: {fullmessage}");
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
            Incomes.Clear();
            Expenses.Clear();

            try
            {
                var loadedTransactions = await _transactionService.GetAllAsync();

                foreach (var transaction in loadedTransactions)
                {
                    Transactions.Add(transaction);
                    if (transaction.TransactionType == TransactionType.Income) Incomes.Add(transaction);
                    if (transaction.TransactionType == TransactionType.Expense) Expenses.Add(transaction);
                }

                OnPropertyChanged(nameof(Expenses));
                OnPropertyChanged(nameof(Incomes));
                OnPropertyChanged(nameof(IncomeSum));
                OnPropertyChanged(nameof(ExpenseSum));
                OnPropertyChanged(nameof(BilanceSum));

                IncomesView.Refresh();
                ExpensesView.Refresh();
            }
            catch (Exception ex)
            {
                var fullmessage = ex.InnerException?.Message ?? ex.Message;
                _notification.Error($"Fehler beim Laden der Transaktion: {fullmessage}");
            }
        }

        private Transaction CloneTransaction(Transaction originalTransaction)
        {
            return new Transaction
            {
                Id = originalTransaction.Id,
                Name = originalTransaction.Name,
                Description = originalTransaction.Description,
                Date = originalTransaction.Date,
                Category = originalTransaction.Category,
                Amount = originalTransaction.Amount,
                BudgetAmount = originalTransaction.BudgetAmount,
                TransactionType = originalTransaction.TransactionType,
                CategoryId = originalTransaction.CategoryId,
            };
        }

        #endregion
    }
}
