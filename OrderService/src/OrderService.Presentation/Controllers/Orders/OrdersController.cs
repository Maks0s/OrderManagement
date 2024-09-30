using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.CQRS.Queries.GetById;
using OrderService.Presentation.Common.DTOs.OrderDTOs;
using OrderService.Presentation.Common.Mappers;
using OrderService.Presentation.Controllers.Common;

namespace OrderService.Presentation.Controllers.Orders
{
    [Route("orders")]
    public class OrdersController
        : BaseApiController
    {
        private readonly ISender _sender;
        private readonly OrderMapper _mapper;

        public OrdersController(ISender sender, OrderMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder([FromBody] OrderRequest orderRequest)
        {
            var addCommand = _mapper.MapToAddOrderCommand(orderRequest);

            var addResult = await _sender.Send(addCommand);

            return addResult.Match(
                    added => CreatedAtAction(
                            nameof(GetOrder),
                            new { orderId = added.Id },
                            _mapper.MapToOrderResponse(added)
                        ),
                    errors => Problem(errors)
                );
        }

        [HttpGet]
        [Route("{orderId:guid}")]
        public async Task<ActionResult<OrderResponse>> GetOrder(Guid orderId)
        {
            var getQuery = new GetOrderByIdQuery(orderId);

            var getResult = await _sender.Send(getQuery);

            return getResult.Match(
                    received => Ok(
                            _mapper.MapToOrderResponse(received)
                        ),
                    errors => Problem(errors)
                );
        }
    }
}