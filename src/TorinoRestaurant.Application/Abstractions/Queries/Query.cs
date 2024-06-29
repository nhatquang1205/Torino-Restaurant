using MediatR;

namespace TorinoRestaurant.Application.Abstractions.Queries
{
    public abstract record Query<T> : IRequest<T>;
}