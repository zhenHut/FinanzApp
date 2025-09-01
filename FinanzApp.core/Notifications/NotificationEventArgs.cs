using FinanzApp.core.Notifications;

namespace FinanzApp.Events
{
    public sealed class NotificationEventArgs : EventArgs
    {
        #region Constructor
        public NotificationEventArgs(Notification notification)
        {
            Notification = notification;
        }

        #endregion

        #region Properties
        public Notification Notification { get; }

        #endregion
    }
}
