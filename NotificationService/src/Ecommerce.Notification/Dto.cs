namespace Ecommerce.Notification.Dtos
{
    public record NotificationDto
    (
        Guid Id,
        string Title,
        string Message,
        bool IsRead,
        DateTime CreatedAt,
        Guid UserId
    );

    public record UserDto
    (
        Guid Id,
        string Name
    );

    public record CreateUserDto
    (
        string Name
    );

    public record UpdateUserDto
    (
        Guid Id,
        string Name
    );

    public record CreateNotificationDto
    (
        string Title,
        string Message,
        Guid UserId
    );

    public record UpdateNotificationDto
    (
        Guid Id,
        string Title,
        string Message,
        bool IsRead,
        DateTime CreatedAt,
        Guid UserId
    );
}