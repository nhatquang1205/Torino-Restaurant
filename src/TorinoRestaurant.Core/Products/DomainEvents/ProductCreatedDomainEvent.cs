using TorinoRestaurant.Core.Abstractions.DomainEvents;

namespace TorinoRestaurant.Core.Products.DomainEvents
{
    public sealed record ProductCreatedDomainEvent(
        long Id,
        string Name,
        string Description,
        string VietnameseDescription,
        double Price,
        double CostPrice,
        string ImageUrl,
        long CategoryId,
        bool IsUseForPrinter,
        string Slug
        ) : DomainEvent {}
}