using System.Security.Claims;

namespace Ecommerce.Common.Auth
{
    public interface ITokenRepository<T>
    {
        string GenerateAccessToken(T user, string key, string issuer, string audience);
        Task<string> GenerateRefreshToken(T user, string key, string issuer, string audience);
        string ValidateAccessToken(string token, string key, string issuer, string audience);
        string ValidateRefreshToken(string token, string key, string issuer, string audience);
        Task<ClaimsPrincipal> GetClaimsPrincipalFromToken(string token, string key, string issuer, string audience); 
    }
}