using System.Linq.Expressions;
using Ecommerce.Order.Database;
using Ecommerce.Order.Dtos;
using Ecommerce.Order.Entities;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Order.Repository
{
    public class ModelRepository : IModelRepository
    {
        private readonly DataContext _dbContext;

        public ModelRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto order)
        {
            ArgumentNullException.ThrowIfNull(order, nameof(order));
            try {
                var product = await _dbContext.Products.FindAsync(order.ProductId);
                var user = await _dbContext.Users.FindAsync(order.UserId);
                
                if (product == null || user == null) {
                    throw new Exception("Product or User not found");
                }

                var orderEntity = new OrderEntity
                {
                    UserId = order.UserId,
                    ProductId = order.ProductId,
                    Quantity = order.Quantity,
                    Price = order.Price,
                    Status = order.Status,
                    Address = order.Address,
                    OrderDate = DateTime.UtcNow,
                    Product = product,
                    User = user
                };
                _dbContext.Orders.Add(orderEntity);
                await _dbContext.SaveChangesAsync();
                return orderEntity.AsDto();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            try {
                var order = await _dbContext.Orders.FindAsync(id);
                if (order == null) {
                    throw new ArgumentNullException(nameof(order));
                }
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderDto> GetOrderAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            try {
                var order = await _dbContext.Orders.FindAsync(id);
                if (order == null) {
                    throw new ArgumentNullException(nameof(order));
                }
                return order.AsDto();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            try {
                var orders = await _dbContext.Orders.ToListAsync();
                return orders.Select(order => order.AsDto());
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(string filter)
        {
            try {
                ArgumentNullException.ThrowIfNull(filter, nameof(filter));
                var orders = _dbContext.Orders;
                orders.Where(order => order.Status == filter);
                orders.Where(order => order.UserId == Guid.Parse(filter));
                return (IEnumerable<OrderDto>)await orders.ToListAsync();
            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateOrderAsync(int Id, UpdateOrderDto order)
        {
            ArgumentNullException.ThrowIfNull(order, nameof(order));
            try {
                var orderEntity = await _dbContext.Orders.FindAsync(Id);
                if (orderEntity == null) {
                    throw new ArgumentNullException(nameof(orderEntity));
                }
                orderEntity.UserId = order.UserId;
                orderEntity.ProductId = order.ProductId;
                orderEntity.Quantity = order.Quantity;
                orderEntity.Price = order.Price;
                orderEntity.Status = order.Status;
                orderEntity.Address = order.Address;
                orderEntity.OrderDate = DateTime.UtcNow;

                _dbContext.Entry(orderEntity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}