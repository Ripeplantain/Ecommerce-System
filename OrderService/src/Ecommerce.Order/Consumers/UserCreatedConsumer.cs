using MassTransit;
using Ecommerce.User.Contract;
using Ecommerce.Order.Database;
using Ecommerce.Order.Entities;

namespace Ecommerce.Order.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreated>
    {

        private readonly DataContext _context;

        public UserCreatedConsumer(DataContext context)
        {
            _context = context;
        }

        public Task Consume(ConsumeContext<UserCreated> context)
        {
            var message = context.Message;

            var user = _context.Users.Find(message.Id);
            if (user != null)
            {
                return Task.CompletedTask;
            }

            user = new AppUser
            {
                Id = message.Id,
                Name = message.Name
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}