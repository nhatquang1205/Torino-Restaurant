using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TorinoRestaurant.Application.Authentication.Models
{
    public class LoginRequestEntity
    {
        /// <summary>
        /// Tên đăng nhập, email hoặc số điện thoại
        /// </summary>
        /// <example>0932046296</example>
        [Required(ErrorMessage = "E001")]
        [StringLength(50, ErrorMessage = "E005")]
        [JsonProperty("username")]
        public required string Username { get; init; }

        /// <summary>
        /// Mật khẩu đăng nhập
        /// </summary>
        /// <example>Admin@123</example>
        [Required(ErrorMessage = "E001")]
        [StringLength(50, ErrorMessage = "E005")]
        [JsonProperty("password")]
        public required string Password { get; init; }
    }
}