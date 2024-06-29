using TorinoRestaurant.Core.Abstractions.DomainEvents;

namespace TorinoRestaurant.Core.Users.DomainEvents
{
    public sealed record UserCreatedDomainEvent(long Id, string PhoneNumber, string FullName) : DomainEvent {}
}