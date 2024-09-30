using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace NotificationService.Host.Controllers.Common
{
    [Route("/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        private readonly ILogger<ErrorsController> _logger;
        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger;
        }

        public ActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?
                .Error;

            var problemDetails = new ProblemDetails()
            {
                Title = "Unexpected problems on the server side. Please try again later",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = exception?.Message,
                Instance = exception?.Source
            };

            _logger.LogError(exception, "{@problemDetails}", problemDetails);

            return Problem(
                title: problemDetails.Title,
                detail: problemDetails.Detail,
                instance: problemDetails.Instance,
                statusCode: problemDetails.Status
                );
        }
    }
}