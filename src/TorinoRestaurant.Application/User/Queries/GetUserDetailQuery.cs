using AutoMapper;
using TorinoRestaurant.Application.Abstractions.Queries;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.User.Models;
using TorinoRestaurant.Core.Abstractions.Guards;
using TblUser = TorinoRestaurant.Core.Users.Entities.User;

namespace TorinoRestaurant.Application.User.Queries
{
    public sealed record GetUserDetailQuery(long Id) : Query<UserEntity>;

    public sealed class GetUserDetailQueryHandler(IMapper mapper,
        IRepository<TblUser, long> repository) : QueryHandler<GetUserDetailQuery, UserEntity>(mapper)
    {
        private readonly IRepository<TblUser, long> _repository = repository;

        protected override async Task<UserEntity> HandleAsync(GetUserDetailQuery request)
        {
            var user = await _repository.GetByIdAsync(request.Id);
            Guard.Against.NotFound(user);
            return Mapper.Map<UserEntity>(user);
        }
    }
}