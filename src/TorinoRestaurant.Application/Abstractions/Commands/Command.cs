using MediatR;

namespace TorinoRestaurant.Application.Abstractions.Commands
{
    public abstract record CommandBase<T> : IRequest<T>;
    public abstract record Command : CommandBase<Unit>;
    public abstract record CreateCommand<P> : IRequest<P>;
}