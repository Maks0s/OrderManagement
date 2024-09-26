using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace OrderService.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(
                ILogger<LoggingBehavior<TRequest, TResponse>> logger
            )
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
                TRequest request,
                RequestHandlerDelegate<TResponse> next,
                CancellationToken cancellationToken
            )
        {
            var result = await next();

            if (result.IsError)
            {
                _logger.LogError(
                        "Error occurred while processing - {@requestName}; problem details - {@errorDetails}",
                        typeof(TRequest).Name,
                        result.Errors
                    );
            }

            return result;
        }
    }
}
