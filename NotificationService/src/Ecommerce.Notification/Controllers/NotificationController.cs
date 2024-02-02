using Ecommerce.Notification.Dtos;
using Ecommerce.Notification.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Notification.Controller
{
    [ApiController]
    [Route("notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(IRepository repository, ILogger<NotificationController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotificationsAsync(
            [FromQuery] string? filter = null
        )
        {
            try {
                if (filter != null)
                {
                    var notifications = await _repository.GetAllAsync(x => x.UserId == Guid.Parse(filter));
                    return Ok(notifications);
                }
                var allNotifications = await _repository.GetAllAsync();
                return Ok(allNotifications);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error occured");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDto>> GetNotificationAsync(int id)
        {
            var notification = await _repository.GetByIdAsync(id);

            if (notification is null)
            {
                return NotFound();
            }

            return Ok(notification);
        }

        [HttpPost]
        public async Task<ActionResult<NotificationDto>> CreateNotificationAsync(CreateNotificationDto notificationDto)
        {
            var notification = await _repository.AddAsync(notificationDto);
            return CreatedAtAction(nameof(GetNotificationAsync), new { id = notification.Id }, notification);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateNotificationAsync(int id, UpdateNotificationDto notificationDto)
        {
            try {
                await _repository.UpdateAsync(id, notificationDto);
                return NoContent();
            } catch (Exception ex) {
                _logger.LogError(ex, "Error occured");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNotificationAsync(int id)
        {
            try {
                await _repository.DeleteAsync(id);
                return NoContent();
            } catch (Exception ex) {
                _logger.LogError(ex, "Error occured");
                return BadRequest();
            }
        }
    }
}