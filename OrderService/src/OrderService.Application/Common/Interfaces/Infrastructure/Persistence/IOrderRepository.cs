using OrderService.Domain.Entities;

namespace OrderService.Application.Common.Interfaces.Infrastructure.Persistence
{
    public interface IOrderRepository
    {
        public Task<Order?> AddOrderAsync(Order orderToAdd);

        public ValueTask<Order?> GetOrderAsync(Guid orderId);
    }
}
