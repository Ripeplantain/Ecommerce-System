using Ecommerce.User.Dto;
using Ecommerce.User.Entities;

namespace Ecommerce.User
{
    public static class Extensions
    {
        public static UserDto AsDto(this AppUser user)
        {
            return new UserDto(
                user.Id,
                user.UniqueId, 
                user.FullName, 
                user.Email ?? string.Empty, 
                user.UserName ?? string.Empty, 
                user.PhoneNumber ?? string.Empty
            );
        }
    }
}