using ErrorOr;
using MediatR;

namespace OrderService.Application.Common.Interfaces.Application.CQRS
{
    public interface ICommand<TResponse>
        : IRequest<ErrorOr<TResponse>>;
}
