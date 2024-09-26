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
        [StringLength(100, ErrorMessage = "E005")]
        [JsonProperty("description")]
        public string Description { get; init; }

        [JsonProperty("image")]
        public IFormFile? Image { get; set; }

        [JsonProperty("base64Image")]
        public string Base64Image { get; set; } = string.Empty;

        [JsonProperty("imageName")]
        public string ImageName { get; set; } = string.Empty!;

        [JsonProperty("isDeleteImage")]
        public bool IsDeleteImage { get; init; }
    }
}