namespace TorinoRestaurant.Application.User.Models
{
    public class UserEntity
    {
        public long Id { get; init; }
        public required string FullName { get; init; }
        public required string PhoneNumber { get; init; }
    }
}