using OrderService.Application.Orders.CQRS.Commands.Add;
using OrderService.Domain.Entities;
using OrderService.Presentation.Common.DTOs.OrderDTOs;
using Riok.Mapperly.Abstractions;

namespace OrderService.Presentation.Common.Mappers
{
    [Mapper]
    public partial class OrderMapper
    {
        public partial AddOrderCommand MapToAddProductCommand(OrderRequest orderRequest);

        public partial OrderResponse MapToProductResponse(Order order);
    }
}