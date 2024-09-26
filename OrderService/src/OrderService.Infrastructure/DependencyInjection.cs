using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderService.Application.Common.Interfaces.Infrastructure.Persistence;
using OrderService.Infrastructure.Persistence.Common;
using OrderService.Infrastructure.Persistence.DbContexts;
using OrderService.Infrastructure.Persistence.Repositories;
using OrderService.Infrastructure.RabbitMq.Common;

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
            services.AddMassTransitConfigurations(configuration);

            return services;
        }

        private static IServiceCollection AddPersistence(
                this IServiceCollection services,
                IConfiguration configuration
            )
        {
            services.AddDbContext<OrderDbContext>(options =>
                options.UseSqlServer(
                        configuration.GetConnectionString("DefaultOrdersMsSql"),
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsHistoryTable(
                                    HistoryRepository.DefaultTableName,
                                    DbSchemaConstants.OrderDbSchema
                                );
                        }
                    )
            );

            services.AddScoped<IOrderRepository, OrderRepository>();

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