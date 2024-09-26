using OrderService.Application.Common.Interfaces.Infrastructure.Persistence;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence.DbContexts;

namespace OrderService.Infrastructure.Persistence.Repositories
{
    public class OrderRepository
        : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order?> AddOrderAsync(Order orderToAdd)
        {
            var addedOrder =
                await _dbContext.Orders.AddAsync(orderToAdd);

            await _dbContext.SaveChangesAsync();

            return addedOrder.Entity;
        }

        public ValueTask<Order?> GetOrderAsync(Guid orderId)
        {
            var order = _dbContext.Orders.FindAsync(orderId);

            return order;
        }
    }
}