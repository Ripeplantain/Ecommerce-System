namespace Ecommerce.Notification.Contract
{
    public record CreateNotification(
        string Title,
        string Message,
        Guid UserId
    );
}