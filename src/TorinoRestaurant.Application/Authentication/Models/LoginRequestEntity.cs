using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TorinoRestaurant.Application.Authentication.Models
{
    public class LoginRequestEntity
    {
        /// <summary>
        /// Tên đăng nhập, email hoặc số điện thoại
        /// </summary>
        /// <example>admin</example>
        [Required(ErrorMessage = "E001")]
        [StringLength(50, ErrorMessage = "E005")]
        [JsonProperty("username")]
        public required string Username { get; init; }

        /// <summary>
        /// Mật khẩu đăng nhập đã mã hoá MD5
        /// </summary>
        /// <example>628c709b5d084dc6b22a6dbe87665419</example>
        [Required(ErrorMessage = "E001")]
        [StringLength(50, ErrorMessage = "E005")]
        [JsonProperty("password")]
        public required string Password { get; init; }
    }
}