using Ecommerce.Notification.Dtos;
using Ecommerce.Notification.Entities;

namespace Ecommerce.Notification
{
    public static class Extension
    {
        public static NotificationDto AsDto(this NotificationEntity notification)
        {
            return new NotificationDto(
                notification.Id, 
                notification.Title, 
                notification.Message, 
                notification.IsRead, 
                notification.CreatedAt, 
                notification.UserId);
        }
    }
}