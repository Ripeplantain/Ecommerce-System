using Ecommerce.Catalog.Dtos;
using Ecommerce.Catalog.Entities;



namespace Ecommerce.Catalog
{
    public static class Extensions
    {
        public static ProductDto AsDto(this Product product)
        {
            return new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Quantity,
                product.IsAvailable,
                product.CreatedAt
            );
        }
    }
}