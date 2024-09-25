using TorinoRestaurant.Core.Abstractions.Entities;
using TorinoRestaurant.Core.Products.DomainEvents;

namespace TorinoRestaurant.Core.Products.Entities
{
    public class ProductPrice : AggregateRoot
    {
        public string Name { get; set; }

        public long ProductId { get; set; }

        public double Price { get; set; }

        public Product Product { get; set; }
    }
}