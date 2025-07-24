using FinanzApp.core.Infrastructure;

namespace FinanzApp.core.Interface
{
    public interface IDialogRequestClose
    {
        event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
    }
}
