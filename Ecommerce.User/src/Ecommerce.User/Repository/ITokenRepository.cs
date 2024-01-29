using System.Security.Claims;
using Ecommerce.User.Entities;

namespace Ecommerce.User.Repository
{
    public interface ITokenRepository
    {
        string GenerateAccessToken(AppUser user);
        Task<string> GenerateRefreshToken(AppUser user);
        Task ValidateRefreshToken(string token);
        Task<ClaimsPrincipal> GetClaimsPrincipalFromToken(string token); 
    }
}