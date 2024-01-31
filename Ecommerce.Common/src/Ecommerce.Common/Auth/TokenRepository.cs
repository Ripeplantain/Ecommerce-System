using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Common.Auth
{

    public class TokenRepository<T> : ITokenRepository<T> where T : IUser
    {
        public string GenerateAccessToken(T user, string key, string issuer, string audience)
        {
            var claims = UserClaims(user, "access_token");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static List<Claim> UserClaims(T user, string tokenType)
        {
            if (tokenType != "access_token" && tokenType != "refresh_token")
                throw new ArgumentException("Invalid token type", nameof(tokenType)
            );

            if (tokenType == "access_token")
            {
                var claims = new List<Claim> {
                    new(ClaimTypes.NameIdentifier, user.UniqueId.ToString()),
                    new(ClaimTypes.Name, user.FullName),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.MobilePhone, user.PhoneNumber),
                    new("TokenType", tokenType)
                };

                return claims;
            } else {
                var claims = new List<Claim> {
                    new(ClaimTypes.NameIdentifier, user.UniqueId.ToString()),
                    new("TokenType", tokenType)
                };

                return claims;
            }
        }

        public Task<string> GenerateRefreshToken(T user, string key, string issuer, string audience)
        {
            var claims = UserClaims(user, "refresh_token");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));

        }

        public async Task<ClaimsPrincipal> GetClaimsPrincipalFromToken(string token, string key, string issuer, string audience)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? string.Empty));

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var claimsIdentity = new ClaimsIdentity(((JwtSecurityToken)validatedToken).Claims);
            return await Task.FromResult(new ClaimsPrincipal(claimsIdentity));
        }

        public string ValidateAccessToken(string token, string key, string issuer, string audience)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? string.Empty));

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;            
            var tokenType = jwtToken.Claims.First(x => x.Type == "TokenType").Value;
            var refreshToken = tokenType == "access_token" ? token : throw new SecurityTokenException("Invalid token type");

            var expiration = jwtToken.ValidTo;
            if (expiration <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Access token has expired");
            }

            return userId;
        }

        public string ValidateRefreshToken(string token, string key, string issuer, string audience)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? string.Empty));

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;            
            var tokenType = jwtToken.Claims.First(x => x.Type == "TokenType").Value;
            var refreshToken = tokenType == "refresh_token" ? token : throw new SecurityTokenException("Invalid token type");

            var expiration = jwtToken.ValidTo;
            if (expiration <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Refresh token has expired");
            }

            return userId;
        }
    }
}