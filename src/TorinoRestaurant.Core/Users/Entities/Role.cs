using TorinoRestaurant.Core.Abstractions.Entities;

namespace TorinoRestaurant.Core.Users.Entities
{
    public class Role : AggregateRoot<string>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public List<RoleOfUser> Users { get; set; } = [];
    }
}