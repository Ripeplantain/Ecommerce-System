using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;


namespace Ecommerce.Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(
            this IServiceCollection services
        ){
            services.AddMassTransit(config =>
            {
                config.AddConsumers(Assembly.GetEntryAssembly());

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("localhost");
                    cfg.ConfigureEndpoints(ctx);
                    cfg.UseMessageRetry(r => 
                    {
                        r.Interval(3, TimeSpan.FromSeconds(10));
                    
                    });
                });
            });

            services.AddHostedService<MassTransitHostedService>();

            return services;
        }
    }
}