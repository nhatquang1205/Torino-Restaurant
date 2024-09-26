using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TorinoRestaurant.Application.Products.Models
{
    public class ProductCreateUpdateEntity
    {
        public ProductCreateUpdateEntity()
        {
            ProductPrices = [];
            DeletedProductPrices = [];
        }
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        /// <example>Pizza Torino</example>
        [Required(ErrorMessage = "E001")]
        [StringLength(256, ErrorMessage = "E005")]
        [JsonProperty("name")]
        public required string Name { get; init; }

        /// <summary>
        /// Danh mục
        /// </summary>
        /// <example>Antipasti</example>
        [Required(ErrorMessage = "E001")]
        [JsonProperty("categoryId")]
        public required long CategoryId { get; init; }

        /// <summary>
        /// Mô tả danh mục
        /// </summary>
        /// <example>Mô tả</example>
        [StringLength(256, ErrorMessage = "E005")]
        [JsonProperty("description")]
        public required string Description { get; init; }

        /// <summary>
        /// Mô tả danh mục
        /// </summary>
        /// <example>Món khai vị</example>
        [StringLength(256, ErrorMessage = "E005")]
        [JsonProperty("vietnameseDescription")]
        public required string VietnameseDescription { get; init; }

        [JsonProperty("productPrices")]
        public required List<ProductPriceEntity> ProductPrices { get; init; }

        [JsonProperty("deletedProductPrices")]
        public List<long> DeletedProductPrices { get; init; }

        [JsonProperty("costPrice")]
        public required double CostPrice { get; init; }

        [JsonProperty("image")]
        public IFormFile? Image { get; set; }

        [JsonProperty("base64Image")]
        public string Base64Image { get; set; } = string.Empty;

        [JsonProperty("imageName")]
        public string ImageName { get; set; } = string.Empty;

        [JsonProperty("isUseForPrinter")]
        public bool IsUseForPrinter { get; init; }

        [JsonProperty("isDeleteImage")]
        public bool IsDeleteImage { get; init; }
    }

    public class ProductPriceEntity
    {
        [JsonProperty("id")]
        public long? Id { get; init; }

        [JsonProperty("name")]
        public required string Name { get; init; }

        [JsonProperty("price")]
        public required double Price { get; init; }
    }
}