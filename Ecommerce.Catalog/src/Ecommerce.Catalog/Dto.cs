using System.ComponentModel.DataAnnotations;



namespace Ecommerce.Catalog.Dtos
{
    public record ProductDto (
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Quantity,
        bool IsAvailable,
        DateTime CreatedAt
    );

    public record CreateProductDto (
        [Required] string Name,
        [Required] string Description,
        [Required] decimal Price,
        [Required] int Quantity
    );

    public record UpdateProductDto (
        [Required] string Name,
        [Required] string Description,
        [Required] decimal Price,
        [Required] int Quantity
    );
}