using TorinoRestaurant.Core.Abstractions.DomainEvents;

namespace TorinoRestaurant.Core.Products.DomainEvents
{
    public sealed record CategoryCreatedDomainEvent(long Id, string Name, string Description, string ImageUrl) : DomainEvent {}
}