using Ecommerce.Notification.Database;
using Ecommerce.Notification.Entities;
using Ecommerce.User.Contract;
using MassTransit;

namespace  Ecommerce.Notification.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreated>
    {
        private readonly DataContext _context;

        public UserCreatedConsumer(DataContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            var user = new AppUser
            {
                Id = context.Message.Id,
                Name = context.Message.Name
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}