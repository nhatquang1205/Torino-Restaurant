using MediatR;
using Microsoft.EntityFrameworkCore;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.Common.Models;
using TorinoRestaurant.Application.Common.Extensions;
using TblCategory = TorinoRestaurant.Core.Products.Entities.Category;
using TorinoRestaurant.Application.Categories.Models;

namespace TorinoRestaurant.Application.Categories.Queries
{
    public sealed record GetCategoriesQuery() : ParamsSearch, IRequest<PaginatedList<CategoryEntity>>;
    public sealed class GetCategoriesQueryHandler(
        IRepository<TblCategory, long> repository) : IRequestHandler<GetCategoriesQuery, PaginatedList<CategoryEntity>>
    {
        private readonly IRepository<TblCategory, long> _repository = repository;

        public async Task<PaginatedList<CategoryEntity>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var dbCategories = _repository.GetAll();

            var query = dbCategories
                .Where(x => string.IsNullOrEmpty(request.KeySearch) || x.Name.Contains(request.KeySearch));

            var categories = await query
                .OrderBy(request)
                .Skip(request.PageSize * (request.CurrentPage - 1))
                .Take(request.PageSize)
                .Select(x => new CategoryEntity
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                })
                .ToListAsync(cancellationToken);

            return new PaginatedList<CategoryEntity>(categories, await query.CountAsync(cancellationToken: cancellationToken), request.CurrentPage, request.PageSize);
        }
    }
}