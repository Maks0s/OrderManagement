using ErrorOr;
using System.Net;

namespace OrderService.Application.Common.AppErrors
{
    public static partial class Errors
    {
        public static class Validation
        {
            public static Error ValidationError(
                    string invalidProperty,
                    string errorMessage
                ) =>
                    Error.Custom(
                        (int)HttpStatusCode.BadRequest,
                        "Validation failure",
                        $"{invalidProperty}-{errorMessage}"
                    );
        }
    }
}