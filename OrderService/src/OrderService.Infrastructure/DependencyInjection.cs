using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
                this IServiceCollection services,
                IConfiguration configuration
            )
        {
            services.AddPersistence(configuration);

            return services;
        }

        private static IServiceCollection AddPersistence(
                this IServiceCollection services,
                IConfiguration configuration
            )
        {
            return services;
        }
    }
}