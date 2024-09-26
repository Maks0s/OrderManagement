using ErrorOr;
using MassTransit;
using OrderService.Application.Common.AppErrors;
using OrderService.Application.Common.Interfaces.Application.CQRS;
using OrderService.Application.Common.Interfaces.Infrastructure.Persistence;
using OrderService.Domain.Entities;
using Shared.Contracts;

namespace OrderService.Application.Orders.CQRS.Commands.Add
{
    public class AddOrderCommandHandler
        : ICommandHandler<AddOrderCommand, Order?>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public AddOrderCommandHandler(
                IOrderRepository orderRepository, 
                IPublishEndpoint publishEndpoint
            )
        {
            _orderRepository = orderRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ErrorOr<Order?>> Handle(
                AddOrderCommand command, 
                CancellationToken cancellationToken
            )
        {
            var orderToAdd = new Order()
            {
                Id = Guid.NewGuid(),
                CustomerId = command.CustomerId,
                OrderDate = DateTime.UtcNow,
                ProductQuantity = command.ProductQuantity
            };

            var addedOrder =
                await _orderRepository.AddOrderAsync(orderToAdd);

            if (addedOrder is null)
            {
                return Errors.ServerDataManipulation.NotAdded();
            }

            await _publishEndpoint.Publish(
                new OrderCreatedEvent()
                {
                    Id = addedOrder.Id,
                    CustomerId = addedOrder.CustomerId,
                    OrderDate = addedOrder.OrderDate,
                    ProductQuantity = addedOrder.ProductQuantity
                }
            );

            return addedOrder;
        }
    }
}