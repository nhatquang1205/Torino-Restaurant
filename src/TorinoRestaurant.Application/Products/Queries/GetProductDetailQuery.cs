using AutoMapper;
using TorinoRestaurant.Application.Abstractions.Queries;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.Products.Models;
using TorinoRestaurant.Core.Abstractions.Guards;

namespace TorinoRestaurant.Application.Products.Queries
{
    public sealed record GetProductDetailQuery(long Id) : Query<ProductEntity>;

    public sealed class GetProductDetailQueryHandler(IMapper mapper,
        IProductRepository repository) : QueryHandler<GetProductDetailQuery, ProductEntity>(mapper)
    {
        private readonly IProductRepository _repository = repository;

        protected override async Task<ProductEntity> HandleAsync(GetProductDetailQuery request)
        {
            var product = await _repository.GetProductById(request.Id);
            product = Guard.Against.NotFound(product);
            return new ProductEntity
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                VietnameseDescription = product.VietnameseDescription,
                Price = product.Price,
                CostPrice = product.CostPrice,
                ImageUrl = product.ImageUrl,
                IsUseForPrinter = product.IsUseForPrinter,
                Category = product.Category.Name,
                CategoryId = product.CategoryId,
                Slug = product.Slug,
                SaleCount = product.SaleCount,
            };
        }
    }
}