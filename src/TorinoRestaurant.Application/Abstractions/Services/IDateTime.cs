using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TorinoRestaurant.Application.Abstractions.Services
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}