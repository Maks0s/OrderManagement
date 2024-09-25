﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Infrastructure.Persistence.Common;
using OrderService.Infrastructure.Persistence.DbContexts;

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

            return services;
        }
    }
}