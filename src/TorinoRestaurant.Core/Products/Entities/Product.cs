using TorinoRestaurant.Core.Abstractions.Entities;
using TorinoRestaurant.Core.Products.DomainEvents;

namespace TorinoRestaurant.Core.Products.Entities
{
    public sealed class Product : AggregateRoot
    {
        private Product(
            string name,
            string description,
            string vietnameseDescription,
            double price,
            double costPrice,
            string imageUrl)
        {
            Name = name;
            Description = description;
            VietnameseDescription = vietnameseDescription;
            Price = price;
            CostPrice = costPrice;
            SaleCount = 0;
            ImageUrl = imageUrl;
        }

        #pragma warning disable CS8618 // this is needed for the ORM for serializing Value Objects
        private Product()
        #pragma warning restore CS8618
        {
        }

        public static Product Create(
            string name,
            string description,
            string vietnameseDescription,
            double price,
            double costPrice,
            string imageUrl)
        {
            // validation should go here before the aggregate is created
            // an aggregate should never be in an invalid state
            var product = new Product(name, description, vietnameseDescription, price, costPrice, imageUrl);
            product.PublishCreated();
            return product;
        }

        private void PublishCreated()
        {
            AddDomainEvent(new ProductCreatedDomainEvent(Id, Name, Description, VietnameseDescription, Price, CostPrice, ImageUrl));
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VietnameseDescription { get; set; }
        public double Price { get; set; }
        public double CostPrice { get; set; }
        public string ImageUrl { get; set; }
        public int SaleCount { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; } = default!;
    }
}