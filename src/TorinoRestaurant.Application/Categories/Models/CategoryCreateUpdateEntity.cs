using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TorinoRestaurant.Application.Categories.Models
{
    /// <summary>
    /// CategoryCreateUpdateEntity
    /// </summary>
    public class CategoryCreateUpdateEntity
    {
        /// <summary>
        /// Tên danh mục
        /// </summary>
        /// <example>Antipasti</example>
        [Required(ErrorMessage = "E001")]
        [StringLength(50, ErrorMessage = "E005")]
        [JsonProperty("name")]
        public required string Name { get; init; }

        /// <summary>
        /// Mô tả danh mục
        /// </summary>
        /// <example>Món khai vị</example>
        [Required(ErrorMessage = "E001")]
        [StringLength(50, ErrorMessage = "E005")]
        [JsonProperty("description")]
        public required string Description { get; init; }

        [JsonProperty("image")]
        public IFormFile? Image { get; init; }

        public bool IsDeleteImage { get; init; }
    }
}