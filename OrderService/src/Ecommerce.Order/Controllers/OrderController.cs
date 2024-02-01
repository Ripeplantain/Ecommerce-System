using Ecommerce.Order.Dtos;
using Ecommerce.Order.Entities;
using Ecommerce.Order.Repository;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce.Order.Controllers
{
    [Route("orders")]
    public class OrderController(
        IModelRepository modelRepository
    ) : ControllerBase
    {
        private readonly IModelRepository _modelRepository = modelRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderEntity>>> GetProducts
        (
            [FromQuery] string? filter = null
        ){
            try {
                if (filter != null)
                {
                    var orders = await _modelRepository.GetOrdersAsync(filter);
                    return Ok(orders);
                } else {
                    var orders = await _modelRepository.GetOrdersAsync();
                    return Ok(orders);
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderEntity>> GetProduct(int id)
        {
            try {
                var order = await _modelRepository.GetOrderAsync(id);
                return Ok(order);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderEntity>> CreateOrder (
            [FromBody] CreateOrderDto createOrder
            )
        {
            try {
                var order = await _modelRepository.CreateOrderAsync(createOrder);
                return StatusCode(201, order);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(
            int id,
            [FromBody] UpdateOrderDto updateOrder
        )
        {
            try {
                await _modelRepository.UpdateOrderAsync(id, updateOrder);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(
            int id
        )
        {
            try {
                await _modelRepository.DeleteOrderAsync(id);
                return NoContent();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}