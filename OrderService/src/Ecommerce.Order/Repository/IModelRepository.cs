using Ecommerce.Order.Dtos;


namespace Ecommerce.Order.Repository 
{
    public interface IModelRepository
    {
        Task<IEnumerable<OrderDto>> GetOrdersAsync();
        Task<IEnumerable<OrderDto>> GetOrdersAsync(string filter);
        Task<OrderDto> GetOrderAsync(int id);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto order);
        Task UpdateOrderAsync(int Id, UpdateOrderDto order);
        Task DeleteOrderAsync(int id);
    }
}