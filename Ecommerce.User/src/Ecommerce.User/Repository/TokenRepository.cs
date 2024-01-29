using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.User.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.User.Repository
{
    public class TokenRepository(
        IConfiguration configuration,
        UserManager<AppUser> userManager,
        ILogger<TokenRepository> logger
        ) : ITokenRepository
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly ILogger<TokenRepository> _logger = logger;

        public string GenerateAccessToken(AppUser user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FullName)
            };

            // var roles = await _userManager.GetRolesAsync(user);

            // foreach (var role in roles)
            // {
            //     claims.Add(new Claim(ClaimTypes.Role, role));
            // }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ?? string.Empty));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task ValidateRefreshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ?? string.Empty));

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JwtSettings:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            _logger.LogInformation("User {userId} is requesting a new token", userId);
            
            var user = await _userManager.FindByIdAsync(userId) ?? throw new SecurityTokenException("Invalid token");
        }

        public async Task<ClaimsPrincipal> GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ?? string.Empty));

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JwtSettings:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var claimsIdentity = new ClaimsIdentity(((JwtSecurityToken)validatedToken).Claims);
            return await Task.FromResult(new ClaimsPrincipal(claimsIdentity));
        }

        public async Task<string> GenerateRefreshToken(AppUser user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ?? string.Empty));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: signingCredentials
            );

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}