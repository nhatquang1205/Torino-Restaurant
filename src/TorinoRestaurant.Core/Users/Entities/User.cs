using TorinoRestaurant.Core.Abstractions.Entities;
using TorinoRestaurant.Core.Orders.Entities;
using TorinoRestaurant.Core.Users.DomainEvents;

namespace TorinoRestaurant.Core.Users.Entities
{
    public sealed class User : AggregateRoot
    {
        private User(string phoneNumber, string fullName, string imageUrl, string password)
        {
            PhoneNumber = phoneNumber;
            FullName = fullName;
            ImageUrl = imageUrl;
            Password = password;
        }

        private User()
        {
        }

        public static User Create(string phoneNumber, string fullName, string imageUrl, string password)
        {
            // validation should go here before the aggregate is created
            // an aggregate should never be in an invalid state
            var user = new User(phoneNumber, fullName, imageUrl, password);
            user.PublishCreated();
            return user;
        }

        public void UpdateRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }

        public void SetPassword(string password)
        {
            Password = password;
        }

        private void PublishCreated()
        {
            AddDomainEvent(new UserCreatedDomainEvent(Id, PhoneNumber, FullName));
        }

        public string PhoneNumber { get; private set; }

        public string FullName { get; private set; }

        public string ImageUrl { get; private set; }

        public string? RefreshToken { get; private set; }

        public string Password { get; private set; }

        public DateTime? RefreshTokenExpiryTime { get; private set; }

        public List<RoleOfUser> Roles { get; set; } = [];

        public List<DineInTableHistory> StaffTableHistories { get; set; } = [];

        public List<DineInTableHistory> CustomerTableHistories { get; set; } = [];
    }
}