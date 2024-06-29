using MediatR;
using Microsoft.EntityFrameworkCore;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.Common.Models;
using TorinoRestaurant.Application.Common.Extensions;
using TorinoRestaurant.Application.User.Models;
using TblUser = TorinoRestaurant.Core.Users.Entities.User;

namespace TorinoRestaurant.Application.User.Queries
{
    public sealed record GetUsersQuery() : ParamsSearch, IRequest<PaginatedList<UserEntity>>;
    public sealed class GetUsersQueryHandler(
        IRepository<TblUser> repository) : IRequestHandler<GetUsersQuery, PaginatedList<UserEntity>>
    {
        private readonly IRepository<TblUser> _repository = repository;

        public async Task<PaginatedList<UserEntity>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var dbUsers = _repository.GetAll();

            var query = dbUsers
                .Where(x => string.IsNullOrEmpty(request.KeySearch) || x.FullName.Contains(request.KeySearch) || x.PhoneNumber.Contains(request.KeySearch));

            var users = await query
                .OrderBy(request)
                .Skip(request.PageSize * (request.CurrentPage - 1))
                .Take(request.PageSize)
                .Select(x => new UserEntity
                {
                    FullName = x.FullName,
                    PhoneNumber = x.PhoneNumber,
                })
                .ToListAsync(cancellationToken);

            return new PaginatedList<UserEntity>(users, await query.CountAsync(), request.CurrentPage, request.PageSize);
        }
    }
}