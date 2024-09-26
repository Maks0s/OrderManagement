using ErrorOr;
using MediatR;

namespace OrderService.Application.Common.Interfaces.Application.CQRS
{
    public interface IQueryHandler<TQuery, TResponse>
        : IRequestHandler<TQuery, ErrorOr<TResponse>>
        where TQuery : IQuery<TResponse>;
}
