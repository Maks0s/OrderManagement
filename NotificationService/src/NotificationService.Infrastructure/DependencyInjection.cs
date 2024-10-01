using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NotificationService.Infrastructure.RabbitMq.Common;

namespace NotificationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
                this IServiceCollection services,
                IConfiguration configuration
            )
        {
            services.AddMassTransitConfigurations(configuration);

            return services;
        }

        private static IServiceCollection AddMassTransitConfigurations(
               this IServiceCollection services,
               IConfiguration configuration
           )
        {
            var rabbitMqConfig = new RabbitMqConfig();
            configuration.Bind(RabbitMqConfig.SectionName, rabbitMqConfig);
            services.AddSingleton(Options.Create(rabbitMqConfig));

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumers(typeof(DependencyInjection).Assembly);

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(rabbitMqConfig.Host, "/", host =>
                    {
                        host.Username(rabbitMqConfig.Username);
                        host.Password(rabbitMqConfig.Password);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}