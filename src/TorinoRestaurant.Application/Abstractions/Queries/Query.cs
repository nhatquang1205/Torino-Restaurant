using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TorinoRestaurant.Application.Abstractions.Queries
{
    public abstract record Query<T> : IRequest<T>;
}