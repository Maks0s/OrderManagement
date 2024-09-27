using MediatR;
using Microsoft.AspNetCore.Mvc;
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
            var addCommand = _mapper.MapToAddProductCommand(orderRequest);

            var addResult = await _sender.Send(addCommand);

            return addResult.Match(
                    //TODO: After create 'GetOrder' replace
                    // Created() with CreatedAtAction()
                    added => Created(
                            "uri",
                            _mapper.MapToProductResponse(added)
                        ),
                    errors => Problem(errors)
                );
        }
    }
}