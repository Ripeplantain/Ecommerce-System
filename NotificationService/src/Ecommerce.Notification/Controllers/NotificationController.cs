using Ecommerce.Notification.Dtos;
using Ecommerce.Common;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Notification.Entities;
using System.Linq.Expressions;
using MassTransit.Initializers;

namespace Ecommerce.Notification.Controller
{
    [ApiController]
    [Route("notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly IRepository<NotificationEntity> _repository;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(IRepository<NotificationEntity> repository, ILogger<NotificationController> logger)
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
                Expression<Func<NotificationEntity, bool>> filterExpression = x => true;
                if (filter != null)
                {
                    filterExpression = x => x.UserId == Guid.Parse(filter);
                }

                var notifications = (await _repository.GetAllAsync(filterExpression))
                                        .ToList()
                                        .Select(notification => notification.AsDto());
                return Ok(notifications);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error occured");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDto>> GetNotificationAsync(Guid id)
        {
            try {
                var notification = await _repository.GetByIdAsync(id);
                if (notification is null)
                {
                    return NotFound();
                }
                return Ok(notification);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error occured");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<NotificationDto>> CreateNotificationAsync(CreateNotificationDto notificationDto)
        {
            try {
                var notification = new NotificationEntity
                {
                    Id = Guid.NewGuid(),
                    Title = notificationDto.Title,
                    Message = notificationDto.Message,
                    UserId = notificationDto.UserId,
                    CreatedAt = DateTime.Now
                };
                await _repository.CreateAsync(notification);
                return CreatedAtAction(nameof(GetNotificationAsync), new { id = notification.Id }, notification);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error occured");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateNotificationAsync(Guid id, UpdateNotificationDto notificationDto)
        {
            try {
                var existingNotification = await _repository.GetByIdAsync(id);
                if (existingNotification is null)
                {
                    return NotFound();
                }
                existingNotification.Title = notificationDto.Title;
                existingNotification.Message = notificationDto.Message;
                existingNotification.IsRead = notificationDto.IsRead;
                await _repository.UpdateAsync(existingNotification);
                return NoContent();
            } catch (Exception ex) {
                _logger.LogError(ex, "Error occured");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNotificationAsync(Guid id)
        {
            try {
                var existingNotification = await _repository.GetByIdAsync(id);
                if (existingNotification is null)
                {
                    return NotFound();
                }
                await _repository.DeleteAsync(existingNotification.Id);
                return NoContent();
            } catch (Exception ex) {
                _logger.LogError(ex, "Error occured");
                return BadRequest();
            }
        }
    }
}