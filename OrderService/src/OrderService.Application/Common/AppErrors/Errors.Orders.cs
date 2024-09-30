using ErrorOr;
using System.Net;

namespace OrderService.Application.Common.AppErrors
{
    public static partial class Errors
    {
        public static class Orders
        {
            public static Error NotFound(Guid orderId) =>
                Error.Custom(
                        (int)HttpStatusCode.NotFound,
                        "Requested order was not found",
                        $"Requested order with ID: {orderId} was not found. Please correct your request."
                    );
        }
    }
}