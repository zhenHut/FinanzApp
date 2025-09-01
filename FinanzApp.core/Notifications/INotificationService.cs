using FinanzApp.core.Interfaces;
using FinanzApp.Events;

namespace FinanzApp.Interfaces
{


    public interface INotificationService
    {
        #region Field
       public event EventHandler<NotificationEventArgs>? Notified;

        #endregion

        #region Method
        void Info(string message, string? title = null);
        void Success(string message, string? title = null);
        void Warning(string message, string? title = null);
        void Error(string message, string? title = null);
        void Error(Exception ex, string? title = null);
        #endregion
    }
}
