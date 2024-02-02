using Ecommerce.Catalog.Contract;
using Ecommerce.Order.Database;
using MassTransit;



namespace Ecommerce.Order.Consumers
{
    public class ProductDeletedConsumer : IConsumer<ProductDeleted>
    {
        private readonly DataContext _context;

        public ProductDeletedConsumer(DataContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ProductDeleted> context)
        {
            var message = context.Message;
            var product = await _context.Products.FindAsync(message.Id);
            if (product == null)
            {
                return;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}