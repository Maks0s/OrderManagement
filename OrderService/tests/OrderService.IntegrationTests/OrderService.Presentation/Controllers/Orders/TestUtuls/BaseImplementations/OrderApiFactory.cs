using Microsoft.Extensions.DependencyInjection;
using OrderService.Infrastructure.Persistence.DbContexts;
using OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders.TestUtuls.Helpers;
using OrderService.IntegrationTests.OrderService.Presentation.Controllers.TestUtils;

namespace OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders.TestUtuls.BaseImplementations
{
    public class OrderApiFactory
        : BaseApiFactory
    {
        public readonly OrderGenerator OrderGenerator = new OrderGenerator(3);

        public async Task ReseedDbAsync()
        {
            await ResetDbAsync();
            await SeedOrderTableAsync();
        }

        private async Task SeedOrderTableAsync()
        {
            using var scope = Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<OrderDbContext>();

            await context.Orders
                .AddRangeAsync(
                        OrderGenerator.SeededOrders
                    );

            await context.SaveChangesAsync();
        }
    }
}