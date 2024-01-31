using Ecommerce.Common;


namespace Ecommerce.Catalog.Entities
{
    public class Product : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}