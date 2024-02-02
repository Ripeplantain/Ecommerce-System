using Ecommerce.Catalog.Contract;
using Ecommerce.Order.Database;
using Ecommerce.Order.Entities;
using MassTransit;


namespace Ecommerce.Order.Consumers
{
    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        private readonly DataContext _context;

        public ProductCreatedConsumer(DataContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            var message = context.Message;
            var product = new Product
            {
                Id = message.Id,
                Name = message.Name,
                Description = message.Description,
                Price = message.Price
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
    }
}