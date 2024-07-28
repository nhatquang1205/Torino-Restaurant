using TorinoRestaurant.Core.Abstractions.Entities;
using TorinoRestaurant.Core.Products.DomainEvents;

namespace TorinoRestaurant.Core.Products.Entities
{
    public class Category : AggregateRoot
    {
        private Category(string name, string description, string imageUrl)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
        }

        #pragma warning disable CS8618 // this is needed for the ORM for serializing Value Objects
        private Category()
        #pragma warning restore CS8618
        {
        }

        public static Category Create(string name, string description, string imageUrl)
        {
            // validation should go here before the aggregate is created
            // an aggregate should never be in an invalid state
            var category = new Category(name, description, imageUrl);
            category.PublishCreated();
            return category;
        }

        private void PublishCreated()
        {
            AddDomainEvent(new CategoryCreatedDomainEvent(Id, Name, Description, ImageUrl));
        }

        public static readonly string FolderImagePrefix = "categories";

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<Product> Products { get; set; } = [];
    }
}