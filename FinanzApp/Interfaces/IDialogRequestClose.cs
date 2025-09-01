using FinanzApp.core.Infrastructure;

namespace FinanzApp.core.Interfaces
{
    public interface IDialogRequestClose
    {
        event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
    }
}
