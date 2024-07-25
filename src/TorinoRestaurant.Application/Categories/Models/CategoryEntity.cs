namespace TorinoRestaurant.Application.Categories.Models
{
    public class CategoryEntity
    {
        public long Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;
    }
}