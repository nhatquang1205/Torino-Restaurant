using TorinoRestaurant.Application.Common.Models;

namespace TorinoRestaurant.Application.Products.Models
{
    public record SearchCondition : ParamsSearch
    {
        public int? CategoryId { get; set; }
    }
}