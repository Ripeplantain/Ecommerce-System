using System.Linq.Expressions;
using Ecommerce.Notification.Database;
using Ecommerce.Notification.Dtos;
using Ecommerce.Notification.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Notification.Repositories
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public async Task<NotificationDto> AddAsync(CreateNotificationDto notification)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));
            try {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == notification.UserId)
                    ?? throw new Exception("User not found");
                var notificationEntity = new NotificationEntity
                {
                    Title = notification.Title,
                    Message = notification.Message,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow,
                    UserId = user.Id,
                    User = user
                };
                _context.Notifications.Add(notificationEntity);
                await _context.SaveChangesAsync();
                return notificationEntity.AsDto();
            } catch (Exception ex) {
                throw new Exception("Error occured", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("Notification not found");
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<NotificationDto>> GetAllAsync()
        {
            var notifications = _context.Notifications
                .Include(x => x.User);
            return await notifications.Select(x => x.AsDto()).ToListAsync();

        }

        public async Task<IEnumerable<NotificationDto>> GetAllAsync(Expression<Func<NotificationDto, bool>> predicate)
        {
            var notifications = _context.Notifications
                .Include(x => x.User);
            return await notifications.Select(x => x.AsDto()).Where(predicate).ToListAsync();
        }

        public async Task<NotificationDto> GetByIdAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            var notification = await _context.Notifications
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("Notification not found");
            return notification.AsDto();
        }

        public async Task<NotificationDto> UpdateAsync(int id, UpdateNotificationDto notification)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));
            try {
                var notificationEntity = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id)
                    ?? throw new Exception("Notification not found");
                notificationEntity.Title = notification.Title;
                notificationEntity.Message = notification.Message;
                notificationEntity.IsRead = notification.IsRead;
                notificationEntity.CreatedAt = notification.CreatedAt;
                notificationEntity.UserId = notification.UserId;
                await _context.SaveChangesAsync();
                return notificationEntity.AsDto();
            } catch (Exception ex) {
                throw new Exception("Error occured", ex);
            }
        }
    }
}