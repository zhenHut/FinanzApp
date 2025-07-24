using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanzApp.core.Infrastructure;
using FinanzApp.core.Interface;
using FinanzApp.core.Model;
using System.Collections.ObjectModel;

namespace FinanzApp.core.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        #region Constructor

        public MainViewModel(ITransactionService transactionService,IDialogService dialogService)
        {

            _dialogService = dialogService;
            _transactionService = transactionService;
            Transactions = new();
           
            LoadTransactionsAsyncCommand.Execute(null);
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

        #region Properties
        public ObservableCollection<Transaction> Transactions { get;private set; }

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
                Transactions.Add(result);

                if (result.TransactionType == TransactionType.Income && result.Amount > 0)
                {
                    OnPropertyChanged(nameof(IncomeSum));
                    OnPropertyChanged(nameof(Incomes));
                }
                else if (result.TransactionType == TransactionType.Expense && result.Amount > 0)
                    OnPropertyChanged(nameof(ExpenseSum));
                OnPropertyChanged(nameof(Expenses));
            }

            OnPropertyChanged(nameof(BilanceSum));

        }

        [RelayCommand(CanExecute = nameof(CanExecuteTransactionCommands))]
        private void DeleteTransaction(Transaction transaction)
        {
            if (transaction != null)
            {
                Transactions.Remove(transaction);
                LoadOnPropertyChanged();
            }
        }

        [RelayCommand(CanExecute = nameof(CanExecuteTransactionCommands))]
        private void EditTransaction(Transaction transaction)
        {

            if (transaction != null)
            {
                var copyTransaction = new Transaction
                {
                    Name = transaction.Name,
                    Description = transaction.Description,
                    Date = transaction.Date,
                    Category = transaction.Category,
                    Amount = transaction.Amount,
                    BudgetAmount = transaction.BudgetAmount,
                    TransactionType = transaction.TransactionType,

                };
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

                    LoadOnPropertyChanged();

                }
            }
        }

        private bool CanExecuteTransactionCommands(Transaction transaction)
        {
            return transaction != null;
        }

        private void LoadOnPropertyChanged()
        {
            OnPropertyChanged(nameof(IncomeSum));
            OnPropertyChanged(nameof(ExpenseSum));
            OnPropertyChanged(nameof(BilanceSum));
            OnPropertyChanged(nameof(Expenses));
            OnPropertyChanged(nameof(Incomes));

        }

        [RelayCommand]
        private async Task LoadTransactionsAsync()
        {
            Transactions.Clear();

            var loadedTransactions = await _transactionService.GetAllAsync();

            foreach (var transaction in loadedTransactions)
                Transactions.Add(transaction);

            OnPropertyChanged(nameof(IncomeSum));
            OnPropertyChanged(nameof(ExpenseSum));
            OnPropertyChanged(nameof(BilanceSum));
        }

        #endregion
    }
}
