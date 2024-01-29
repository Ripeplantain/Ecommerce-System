using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Common;


namespace Ecommerce.User.Entities
{
    [Table("Users")]
    public class AppUser : IdentityUser
    {
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string FullName { get; set; } = string.Empty;
    }
}