using Ecommerce.Common;
using Ecommerce.Notification.Contract;
using Ecommerce.Notification.Entities;
using MassTransit;


namespace Ecommerce.Notification.Consumers
{
    public class NotificationCreatedConsumer : IConsumer<CreateNotification>
    {

        private readonly IRepository<NotificationEntity> _repository;

        public NotificationCreatedConsumer(IRepository<NotificationEntity> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CreateNotification> context)
        {
            var notification = new NotificationEntity
            {
                Id = Guid.NewGuid(),
                Title = context.Message.Title,
                Message = context.Message.Message,
                UserId = context.Message.UserId,
                CreatedAt = DateTime.Now
            };

            await _repository.CreateAsync(notification);
        }
    }
}