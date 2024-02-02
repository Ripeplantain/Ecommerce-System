using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Order.Entities
{
    [Table("Products")]
    public class Product
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<OrderEntity> Orders { get; set; } = null!;
    }
}