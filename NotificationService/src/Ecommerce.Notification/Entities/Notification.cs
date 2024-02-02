using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Notification.Entities
{
    [Table("Notifications")]
    public class NotificationEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; } = null!;
        [MaxLength(250)]
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public AppUser User { get; set; } = null!;
    }
}
