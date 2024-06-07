using MediatR;

namespace TorinoRestaurant.Core.Abstractions.DomainEvents
{
    public abstract record DomainEvent : INotification
    {
    }
}