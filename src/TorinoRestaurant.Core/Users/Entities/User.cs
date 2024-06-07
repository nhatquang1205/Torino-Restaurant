using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorinoRestaurant.Core.Abstractions.Entities;
using TorinoRestaurant.Core.Users.DomainEvents;

namespace TorinoRestaurant.Core.Users.Entities
{
    public sealed class User : AggregateRoot
    {
        private User(string phoneNumber, string fullName)
        {
            PhoneNumber = phoneNumber;
            FullName = fullName;
        }

        #pragma warning disable CS8618 // this is needed for the ORM for serializing Value Objects
        private User()
        #pragma warning restore CS8618
        {
        }

        public static User Create(string phoneNumber, string fullName)
        {
            // validation should go here before the aggregate is created
            // an aggregate should never be in an invalid state
            var user = new User(phoneNumber, fullName);
            user.PublishCreated();
            return user;
        }

        private void PublishCreated()
        {
            AddDomainEvent(new UserCreatedDomainEvent(Id, PhoneNumber, FullName));
        }

        public string PhoneNumber { get; private set; }

        public string FullName { get; private set; }

    }
}