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

        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            Console.WriteLine("UserCreatedConsumer");
            var message = context.Message;

            var existingUser = await _context.Users.FindAsync(message.Id);
    
            if (existingUser == null)
            {
                return;
            }

            var user = new AppUser
            {
                Id = message.Id,
                Name = message.Name
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}