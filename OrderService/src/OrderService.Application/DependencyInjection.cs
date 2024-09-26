using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common.Behaviors;

namespace OrderService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            return services;
        }
    }
}