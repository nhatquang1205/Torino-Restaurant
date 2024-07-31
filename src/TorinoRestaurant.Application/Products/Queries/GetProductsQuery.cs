using MediatR;
using Microsoft.EntityFrameworkCore;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.Common.Models;
using TorinoRestaurant.Application.Common.Extensions;
using TblCategory = TorinoRestaurant.Core.Products.Entities.Category;
using TorinoRestaurant.Application.Products.Models;

namespace TorinoRestaurant.Application.Products.Queries
{
    public sealed record GetProductsQuery() : ParamsSearch, IRequest<PaginatedList<ProductEntity>>;
    public sealed class GetProductsQueryHandler(
        IProductRepository repository) : IRequestHandler<GetProductsQuery, PaginatedList<ProductEntity>>
    {
        private readonly IProductRepository _repository = repository;

        public async Task<PaginatedList<ProductEntity>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var dbProducts = _repository.GetAll();

            var query = dbProducts
                .Where(x => string.IsNullOrEmpty(request.KeySearch) ||
                    x.Name.Contains(request.KeySearch) ||
                    x.Description.Contains(request.KeySearch) ||
                    x.VietnameseDescription.Contains(request.KeySearch));

            var products = await query
                .OrderBy(request)
                .Skip(request.PageSize * (request.CurrentPage - 1))
                .Take(request.PageSize)
                .Select(x => new ProductEntity
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    VietnameseDescription = x.VietnameseDescription,
                    ImageUrl = x.ImageUrl,
                    Category = x.Category.Name,
                    CategoryId = x.CategoryId,
                    Price = x.Price,
                    CostPrice = x.CostPrice,
                    IsUseForPrinter = x.IsUseForPrinter,
                    Slug = x.Slug,
                    SaleCount = x.SaleCount,
                })
                .ToListAsync(cancellationToken);

            return new PaginatedList<ProductEntity>(products, await query.CountAsync(cancellationToken: cancellationToken), request.CurrentPage, request.PageSize);
        }
    }
}