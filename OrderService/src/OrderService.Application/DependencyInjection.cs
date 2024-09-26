using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common.Behaviors;
using System.Reflection;

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
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            ValidatorOptions.Global.LanguageManager.Enabled = false;

            return services;
        }
    }
}