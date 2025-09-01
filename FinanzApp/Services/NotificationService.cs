using FinanzApp.core.Notifications;
using FinanzApp.Events;
using FinanzApp.Interfaces;

namespace FinanzApp.Services
{
    public class NotificationService : INotificationService
    {
        #region Events

        public event EventHandler<NotificationEventArgs>? Notified;

        #endregion

        #region Methods

        public void Info(string message, string? title = null)
        {
            RaiseNotification(NotificationKind.Info, message, title);
        }

        public void Success(string message, string? title = null)
        {
            RaiseNotification(NotificationKind.Success, message, title);
        }

        public void Warning(string message, string? title = null)
        {
            RaiseNotification(NotificationKind.Warning, message, title);
        }

        public void Error(string message, string? title = null)
        {
            RaiseNotification(NotificationKind.Error, message, title);
        }

        public void Error(Exception ex, string? title = null)
        {
            Error(ex.InnerException?.Message ?? ex.Message, title);
        }

        private void RaiseNotification(NotificationKind kind, string message, string ? title = null, Exception ? exception= null )
            => Notified?.Invoke(this, new NotificationEventArgs(new Notification(kind,message,title,exception)));

        #endregion
    }
}
