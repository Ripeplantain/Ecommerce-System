namespace Ecommerce.Order.Dtos
{
    public record OrderDto
    (
        int Id,
        Guid UserId,
        Guid ProductId,
        int Quantity,
        decimal Price,
        string Status,
        string Address,
        DateTime OrderDate
    );

    public record CreateOrderDto
    (
        Guid UserId,
        Guid ProductId,
        int Quantity,
        decimal Price,
        string Status,
        string Address
    );

    public record UpdateOrderDto
    (
        Guid UserId,
        Guid ProductId,
        int Quantity,
        decimal Price,
        string Status,
        string Address
    );
}