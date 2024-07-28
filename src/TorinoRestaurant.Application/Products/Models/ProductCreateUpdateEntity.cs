using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TorinoRestaurant.Application.Products.Models
{
    public class ProductCreateUpdateEntity
    {
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

        [JsonProperty("price")]
        public required double Price { get; init; }

        [JsonProperty("costPrice")]
        public required double CostPrice { get; init; }

        [JsonProperty("image")]
        public IFormFile? Image { get; init; }

        [JsonProperty("isUseForPrinter")]
        public bool IsUseForPrinter { get; init; }

        [JsonProperty("isDeleteImage")]
        public bool IsDeleteImage { get; init; }
    }
}