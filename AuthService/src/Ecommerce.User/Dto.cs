using System.ComponentModel.DataAnnotations;

namespace Ecommerce.User.Dto
{
    public record UserDto(
        string Id,
        Guid UniqueId,
        [Required] string FullName,
        [Required] string Email,
        [Required] string UserName,
        [Required] string PhoneNumber
    );
    
    public record UserLoginDto(
        [EmailAddress] [Required] string Email,
        [Required] string Password
    );

    public record UserRegisterDto(
        [Required] [MaxLength(50)] string FullName,
        [Required] [EmailAddress] string Email,
        [Required] string Password,
        [Required] string PhoneNumber
    );
}