namespace Ecommerce.Catalog.Contract
{
    public record ProductCreated
    (
        Guid Id,
        string Name,
        string Description,
        decimal Price
    );

    public record ProductUpdated
    (
        Guid Id,
        string Name,
        string Description,
        decimal Price
    );

    public record ProductDeleted
    (
        Guid Id
    );
}