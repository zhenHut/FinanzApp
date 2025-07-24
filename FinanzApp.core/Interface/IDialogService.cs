using FinanzApp.core.Infrastructure;
using FinanzApp.core.Model;


namespace FinanzApp.core.Interface
{
    public interface IDialogService
    {
        Transaction? ShowTransactionDialog(Transaction? transaction= null, TransactionType transactionType = TransactionType.None);
    
    }
}
