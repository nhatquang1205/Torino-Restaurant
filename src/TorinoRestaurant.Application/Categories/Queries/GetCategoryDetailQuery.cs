using AutoMapper;
using TorinoRestaurant.Application.Abstractions.Queries;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.Categories.Models;
using TorinoRestaurant.Application.User.Models;
using TorinoRestaurant.Core.Abstractions.Guards;
using TblCategory = TorinoRestaurant.Core.Products.Entities.Category;

namespace TorinoRestaurant.Application.Categories.Queries
{
    public sealed record GetCategoryDetailQuery(long Id) : Query<CategoryEntity>;

    public sealed class GetCategoryDetailQueryHandler(IMapper mapper,
        IRepository<TblCategory, long> repository) : QueryHandler<GetCategoryDetailQuery, CategoryEntity>(mapper)
    {
        private readonly IRepository<TblCategory, long> _repository = repository;

        protected override async Task<CategoryEntity> HandleAsync(GetCategoryDetailQuery request)
        {
            var category = await _repository.GetByIdAsync(request.Id);
            Guard.Against.NotFound(category);
            return Mapper.Map<CategoryEntity>(category);
        }
    }
}