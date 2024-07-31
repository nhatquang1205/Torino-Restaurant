namespace TorinoRestaurant.Application.Products.Models
{
    public class ProductEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VietnameseDescription { get; set; }
        public long CategoryId { get; set; }
        public string Category { get; set; }
        public double CostPrice { get; set; }
        public double Price { get; set; }
        public bool IsUseForPrinter { get; set; }
        public string ImageUrl { get; set; }
        public string Slug { get; set;}
        public int SaleCount { get; set;}
    }
}