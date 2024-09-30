using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Persistence.DbContexts;
using OrderService.Presentation.Common.Mappers;

namespace OrderService.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddMappers();

            return services;
        }

        private static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddScoped<OrderMapper>();

            return services;
        }

        public static void ApplyDbMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using OrderDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<OrderDbContext>();

            dbContext.Database.Migrate();
        }
    }
}