using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;

namespace OrderService.Presentation.Controllers.Common
{
    [Controller]
    public class BaseApiController : ControllerBase
    {
        protected ActionResult Problem(List<Error> errors)
        {
            if (errors.TrueForAll(error => error.Code.Equals("Validation failure")))
            {
                var msd = new ModelStateDictionary();

                foreach (var error in errors)
                {
                    var invalidFieldAndDescription = error.Description.Split('-');
                    msd.AddModelError(
                            invalidFieldAndDescription[0],
                            invalidFieldAndDescription[1]
                        );
                }

                return ValidationProblem(msd);
            }

            var firstOccurredError = errors[0];

            return Problem(
                    statusCode: firstOccurredError.NumericType,
                    title: firstOccurredError.Code,
                    detail: firstOccurredError.Description,
                    instance: Request.Path.Value
                );
        }
    }
}