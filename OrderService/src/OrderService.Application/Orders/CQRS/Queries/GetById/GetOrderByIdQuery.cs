using OrderService.Application.Common.Interfaces.Application.CQRS;
using OrderService.Domain.Entities;

namespace OrderService.Application.Orders.CQRS.Queries.GetById
{
    public record GetOrderByIdQuery(
            Guid OrderId
        ) : IQuery<Order?>;
}