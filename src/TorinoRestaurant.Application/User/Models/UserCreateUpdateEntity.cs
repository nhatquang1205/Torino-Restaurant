namespace TorinoRestaurant.Application.User.Models
{
    public class UserCreateUpdateEntity
    {
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
    }
}