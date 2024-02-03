using Ecommerce.Catalog.Contract;
using Ecommerce.Order.Database;
using MassTransit;

namespace Ecommerce.Order.Consumers
{
    public class ProductUpdatedConsumer : IConsumer<ProductUpdated>
    {
        private readonly DataContext _context;

        public ProductUpdatedConsumer(DataContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ProductUpdated> context)
        {
            Console.WriteLine("ProductUpdatedConsumer");
            var message = context.Message;
            var product = await _context.Products.FindAsync(message.Id);
            if (product == null)
            {
                return;
            }
            product.Name = message.Name;
            product.Description = message.Description;
            product.Price = message.Price;
            await _context.SaveChangesAsync();
        }
    }
}