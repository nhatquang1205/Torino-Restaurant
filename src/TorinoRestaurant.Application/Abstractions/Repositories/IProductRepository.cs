using TorinoRestaurant.Core.Products.Entities;

namespace TorinoRestaurant.Application.Abstractions.Repositories
{
    public interface IProductRepository : IRepository<Product, long>
    {
        Task<bool> IsExistSlug(long? productId, string slug);
    }
}