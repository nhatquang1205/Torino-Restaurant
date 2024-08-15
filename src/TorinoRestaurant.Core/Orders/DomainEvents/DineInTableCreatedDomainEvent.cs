using TorinoRestaurant.Core.Abstractions.DomainEvents;

namespace TorinoRestaurant.Core.Orders.DomainEvents
{
    public sealed record DineInTableCreatedDomainEvent(long Id, string Name, int Sort) : DomainEvent {}
}