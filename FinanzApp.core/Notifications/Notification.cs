namespace FinanzApp.core.Notifications
{
    public sealed record Notification(
      NotificationKind Kind, string Message, string? Title = null,
      Exception? Exception = null, bool IsModal = false, TimeSpan? Duration = null);
}
