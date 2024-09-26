using TorinoRestaurant.Core.Abstractions.Entities;
using TorinoRestaurant.Core.Orders.Entities;
using TorinoRestaurant.Core.Products.DomainEvents;

namespace TorinoRestaurant.Core.Products.Entities
{
    public sealed class Product : AggregateRoot
    {
        private Product(
            string name,
            string description,
            string vietnameseDescription,
            List<ProductPrice> productPrices,
            double costPrice,
            bool isUseForPrinter)
        {
            Name = name;
            Description = description;
            VietnameseDescription = vietnameseDescription;
            CostPrice = costPrice;
            IsUseForPrinter = isUseForPrinter;
            SaleCount = 0;
            OrderDetails = [];
            ProductPrices = productPrices != null && productPrices.Any() ? productPrices : [];
        }

        private Product()
        {
        }

        public static Product Create(
            string name,
            string description,
            string vietnameseDescription,
            List<ProductPrice> productPrices,
            double costPrice,
            bool isUseForPrinter)
        {
            // validation should go here before the aggregate is created
            // an aggregate should never be in an invalid state
            var product = new Product(name, description, vietnameseDescription, productPrices, costPrice, isUseForPrinter);
            product.PublishCreated();
            return product;
        }

        public static readonly string FolderImagePrefix = "products";

        private void PublishCreated()
        {
            AddDomainEvent(new ProductCreatedDomainEvent(Id, Name, Description, VietnameseDescription, Price, CostPrice, ImageUrl, CategoryId, IsUseForPrinter, Slug));
        }

        public void SetSlug(string slug)
        {
            Slug = slug;
        }

        public void SetCategoryId(long categoryId)
        {
            CategoryId = categoryId;
        }

        public void SetImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VietnameseDescription { get; set; }
        public double Price { get; set; }
        public double CostPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int SaleCount { get; set; }
        public long CategoryId { get; set; }
        public string Slug { get; set; }
        public bool IsUseForPrinter { get; set; } = true;
        public Category Category { get; set; } = default!;
        public ICollection<OrderDetail> OrderDetails{ get; set; }
        public ICollection<ProductPrice> ProductPrices { get; set; }
    }
}