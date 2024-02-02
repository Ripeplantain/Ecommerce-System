using System.Linq.Expressions;
using Ecommerce.Notification.Dtos;



namespace Ecommerce.Notification.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<NotificationDto>> GetAllAsync();
        Task<IEnumerable<NotificationDto>> GetAllAsync(Expression<Func<NotificationDto, bool>> predicate);
        Task<NotificationDto> GetByIdAsync(int id);
        Task<NotificationDto> AddAsync(CreateNotificationDto notification);
        Task<NotificationDto> UpdateAsync(int id, UpdateNotificationDto notification);
        Task DeleteAsync(int id);
    }
}
