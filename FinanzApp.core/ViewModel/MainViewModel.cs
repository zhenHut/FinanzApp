using CommunityToolkit.Mvvm.ComponentModel;
using FinanzApp.core.Infrastructure;
using FinanzApp.core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzApp.core.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        #region Constructor

        public MainViewModel()
        {
            Transactions = new();
            Transactions.Add(new Transaction
            {
                TransactionType = TransactionType.Expense,
                Amount = 10,
                Name = "Private Krankenversicherung"
            }
            );

            Transactions.Add(new Transaction
            {
                TransactionType = TransactionType.Expense,
                Amount = 70,
                Name = "Auto Versicherung"
            }
            );

            Transactions.Add(new Transaction
            {
                TransactionType = TransactionType.Expense,
                Amount = 44,
                Name = "Strom",
            }
            )
            ;

            Transactions.Add(new Transaction
            {
                TransactionType = TransactionType.Income,
                Amount = 1300,
                Name = "Arbeitslosengeld"
            }
            );

            Transactions.Add(new Transaction
            {
                TransactionType = TransactionType.Income,
                Amount = 116,
                Name = "Wohngeld"
            }
            );
            Transactions.Add(new Transaction
            {
                TransactionType = TransactionType.Expense,
                Amount = 550,
                Name = "Miete"
            }
            );
        }

        #endregion

        #region Fields


        #endregion

        #region Properties
        public ObservableCollection<Transaction> Transactions { get; init; }
        public IEnumerable<Transaction> Incomes => Transactions.Where
            (t => t.TransactionType == TransactionType.Income);

        public IEnumerable<Transaction> Expenses => Transactions.Where
            (t => t.TransactionType == TransactionType.Expense);

        
        
        public decimal IncomeSum => Transactions?
                .Where(t => t.TransactionType == TransactionType.Income)
                .Sum(t => t.Amount) ?? 0;

        public decimal ExpenseSum => ( -Transactions?
            .Where(t => t.TransactionType == TransactionType.Expense)
            .Sum(t => t.Amount))?? 0;

        public decimal BilanceSum => IncomeSum + ExpenseSum;




        #endregion

        #region Methods

        private void AddTransaction(Transaction transaction)
        {
            if (transaction != null)
            {
                Transactions.Add(transaction);
            }

            #endregion

        }

        private void RemoveTransaction(Transaction transaction) 
        {
            if(transaction != null)
                Transactions.Remove(transaction);
        }
    }
}
