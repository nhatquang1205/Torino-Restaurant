using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TorinoRestaurant.Application.Abstractions.Commands
{
    public class Command
    {
        public abstract record CommandBase<T> : IRequest<T>;
        public abstract record Command : CommandBase<Unit>;
        public abstract record CreateCommand : IRequest<Guid>;
    }
}