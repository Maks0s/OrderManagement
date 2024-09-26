using ErrorOr;
using FluentValidation;
using MediatR;
using OrderService.Application.Common.AppErrors;

namespace OrderService.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr

    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(
                TRequest request,
                RequestHandlerDelegate<TResponse> next,
                CancellationToken cancellationToken
            )
        {
            if (_validator is null)
            {
                return await next();
            }

            var validationResult =
                await _validator.ValidateAsync(request);

            if (validationResult.IsValid)
            {
                return await next();
            }

            return (dynamic)validationResult.Errors
                    .ConvertAll(error =>
                        Errors.Validation.ValidationError(
                                error.PropertyName,
                                error.ErrorMessage
                            )
                );
        }
    }
}