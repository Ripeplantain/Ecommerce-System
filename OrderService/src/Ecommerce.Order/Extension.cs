using Ecommerce.Order.Dtos;
using Ecommerce.Order.Entities;

namespace Ecommerce.Order
{
    public static class Extension
    {
        public static OrderDto AsDto(this OrderEntity order)
        {
            return new OrderDto(
                order.Id,
                order.UserId,
                order.ProductId,
                order.Quantity,
                order.Price,
                order.Status,
                order.Address,
                order.OrderDate
            );
        }
    }
}