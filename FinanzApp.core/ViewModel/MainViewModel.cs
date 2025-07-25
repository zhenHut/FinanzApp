using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanzApp.core.Infrastructure;
using FinanzApp.core.Interface;
using FinanzApp.core.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FinanzApp.core.ViewModel
{
    public partial class MainViewModel : ObservableObject, INotificationRequest
    {
        #region Constructor

        public MainViewModel(ITransactionService transactionService, IDialogService dialogService)
        {

            _dialogService = dialogService;
            _transactionService = transactionService;
            Transactions = new();
            RefreshTransactionsCommand.Execute(null);


        }

        #endregion

        #region Fields

        private IDialogService _dialogService;
        private ITransactionService _transactionService;

        [ObservableProperty]
        private Transaction? _selectedIncome;

        [ObservableProperty]
        private Transaction? _selectedExpense;

        #endregion

        #region Events

        public event EventHandler<string>? NotificationRequested;

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
                    Notify($"Fehler beim hinzufügen der Transaktion: {fullmessage}");
                    Notify($"SQL-Fehler:\n{ex.GetBaseException().Message}");
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
                Notify($"Fehler beim löschen der Transaktion: {fullmessage}");
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
                    Notify($"Fehler beim Aktualisieren der Transaktion: {fullmessage}");
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
                Notify($"Fehler beim Laden der Transaktion: {fullmessage}");
            }
        }


        private void Notify(string message)
        {
            NotificationRequested?.Invoke(this, message);
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
