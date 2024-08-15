using TorinoRestaurant.Core.Abstractions.DomainEvents;

namespace TorinoRestaurant.Core.Orders.DomainEvents
{
    public sealed record DineInTableHistoryCreatedDomainEvent(long Id, long TableId, DateTimeOffset TimeStart, long StaffId, long? CustomerId) : DomainEvent {}
}