namespace Ecommerce.User.Constant
{
    public class Constants
    {
        private readonly IConfiguration _configuration;

        public Constants(IConfiguration configuration)
        {
            _configuration = configuration;
            JwtKey = _configuration["JwtSettings:Key"] ?? string.Empty;
            JwtIssuer = _configuration["JwtSettings:Issuer"] ?? string.Empty;
            JwtAudience = _configuration["JwtSettings:Audience"] ?? string.Empty;
        }

        public string JwtKey { get; } 
        public string JwtIssuer { get; }
        public string JwtAudience { get; }
    }
}