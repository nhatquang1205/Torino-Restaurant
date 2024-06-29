using AutoMapper;
using TorinoRestaurant.Application.Abstractions.Queries;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.User.Models;
using TorinoRestaurant.Core.Abstractions.Guards;
using TblUser = TorinoRestaurant.Core.Users.Entities.User;

namespace TorinoRestaurant.Application.User.Queries
{
    public sealed record GetUserDetailQuery(Guid Id) : Query<UserEntity>;

    public sealed class GetUserDetailQueryHandler : QueryHandler<GetUserDetailQuery, UserEntity>
    {
        private readonly IRepository<TblUser> _repository;

        public GetUserDetailQueryHandler(IMapper mapper,
            IRepository<TblUser> repository) : base(mapper)
        {
            _repository = repository;
        }

        protected override async Task<UserEntity> HandleAsync(GetUserDetailQuery request)
        {
            var user = await _repository.GetByIdAsync(request.Id);
            Guard.Against.NotFound(user);
            return Mapper.Map<UserEntity>(user);
        }
    }
}