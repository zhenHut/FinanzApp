using FinanzApp.core.Infrastructure;
using FinanzApp.core.Interface;
using FinanzApp.View;

using FinanzApp.core.Model;

namespace FinanzApp.core.Services
{
    public class DialogService : IDialogService
    {
        public Transaction? ShowTransactionDialog(Transaction? transaction, TransactionType transactionType = TransactionType.None)
        {
            var transact = transaction ?? new Transaction();

            if (transactionType != TransactionType.None)
                transact.TransactionType = transactionType;

            var dialog = new TransactionDetail(transact);
            var result = dialog.ShowDialog();

           
            return result == true? transact : null;
        }
    }
}
