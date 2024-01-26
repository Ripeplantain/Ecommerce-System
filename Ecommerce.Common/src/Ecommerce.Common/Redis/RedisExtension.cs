using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;



namespace Ecommerce.Common.Redis
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "master";
            });

            return services;
        }
    }
}