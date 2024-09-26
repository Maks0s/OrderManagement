using ErrorOr;
using MediatR;

namespace OrderService.Application.Common.Interfaces.Application.CQRS
{
    public interface ICommandHandler<TCommand, TResponse>
        : IRequestHandler<TCommand, ErrorOr<TResponse>>
        where TCommand : ICommand<TResponse>;
}
