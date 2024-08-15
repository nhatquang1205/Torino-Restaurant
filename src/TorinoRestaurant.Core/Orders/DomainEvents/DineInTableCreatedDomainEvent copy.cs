using TorinoRestaurant.Core.Abstractions.DomainEvents;

namespace TorinoRestaurant.Core.Orders.DomainEvents
{
    public sealed record OrderCreatedDomainEvent(long Id, string Code, decimal TotalPrice) : DomainEvent {}
}