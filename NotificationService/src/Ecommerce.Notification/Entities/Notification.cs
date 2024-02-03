using Ecommerce.Common;


namespace Ecommerce.Notification.Entities
{
    public class NotificationEntity : IEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
    }
}
