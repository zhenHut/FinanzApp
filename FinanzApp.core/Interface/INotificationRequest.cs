namespace FinanzApp.core.Interface
{
    public interface INotificationRequest
    {
        event EventHandler<string>? NotificationRequested;
    }
}
