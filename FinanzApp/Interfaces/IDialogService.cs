using FinanzApp.core.Models;


namespace FinanzApp.core.Interfaces
{
    public interface IDialogService
    {
        Transaction? ShowTransactionDialog(Transaction? transaction= null, TransactionType transactionType = TransactionType.None);
    
    }
}
