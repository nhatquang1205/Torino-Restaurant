using TorinoRestaurant.Core.Abstractions.Entities;

namespace TorinoRestaurant.Core.Users.Entities
{
    public class RoleOfUser : AggregateRoot
    {
        public required string RoleId { get; set; }
        public long UserId { get; set; }
        public User User { get; set; } = default!;
        public Role Role { get; set; } = default!;
    }
}