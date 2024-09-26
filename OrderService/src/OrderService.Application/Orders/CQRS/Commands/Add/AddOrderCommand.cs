using OrderService.Application.Common.Interfaces.Application.CQRS;
using OrderService.Domain.Entities;

namespace OrderService.Application.Orders.CQRS.Commands.Add
{
    public record AddOrderCommand(
            Guid CustomerId,
            int ProductQuantity
        ) : ICommand<Order?>;
}