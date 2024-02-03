using Ecommerce.Catalog.Contract;
using Ecommerce.Order.Database;
using Ecommerce.Order.Entities;
using MassTransit;


namespace Ecommerce.Order.Consumers
{
    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        private readonly DataContext _context;
        private readonly ILogger<ProductCreatedConsumer> _logger;

        public ProductCreatedConsumer(DataContext context, ILogger<ProductCreatedConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            _logger.LogInformation("Started creating product");
            var message = context.Message;

            var existingProduct = await _context.Products.FindAsync(message.Id);

            if (existingProduct != null)
            {
                return;
            }

            var product = new Product
            {
                Id = message.Id,
                Name = message.Name,
                Description = message.Description,
                Price = message.Price,
                CreatedAt = DateTime.Now
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
    }
}