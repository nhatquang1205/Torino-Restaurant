using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorinoRestaurant.Core.Abstractions.DomainEvents;

namespace TorinoRestaurant.Core.Users.DomainEvents
{
    public sealed record UserCreatedDomainEvent(long Id, string PhoneNumber, string FullName) : DomainEvent {}
}