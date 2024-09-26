using ErrorOr;
using MediatR;

namespace OrderService.Application.Common.Interfaces.Application.CQRS
{
    public interface IQuery<TResponse>
        : IRequest<ErrorOr<TResponse>>;
}
