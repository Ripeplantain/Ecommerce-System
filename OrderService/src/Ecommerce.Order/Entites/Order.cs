using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Order.Entities
{
    [Table("Orders")]
    public class OrderEntity
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Status { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public DateTime OrderDate { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}