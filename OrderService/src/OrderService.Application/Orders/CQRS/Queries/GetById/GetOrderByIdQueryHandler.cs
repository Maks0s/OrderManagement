using ErrorOr;
using OrderService.Application.Common.Interfaces.Application.CQRS;
using OrderService.Application.Common.Interfaces.Infrastructure.Persistence;
using OrderService.Domain.Entities;
using OrderService.Application.Common.AppErrors;

namespace OrderService.Application.Orders.CQRS.Queries.GetById
{
    public class GetOrderByIdQueryHandler
        : IQueryHandler<GetOrderByIdQuery, Order?>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<ErrorOr<Order?>> Handle(
                GetOrderByIdQuery query, 
                CancellationToken cancellationToken
            )
        {
            var order =
                await _orderRepository.GetOrderAsync(query.OrderId);

            if (order is null)
            {
                return Errors.Orders.NotFound(query.OrderId);
            }

            return order;
        }
    }
}