using Microsoft.EntityFrameworkCore;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Core.Products.Entities;

namespace TorinoRestaurant.Infrastructure.Repositories
{
    internal class ProductRepository(DataContext context) : Repository<Product, long>(context), IProductRepository
    {
        public async Task<bool> IsExistSlug(long? productId, string slug)
        {
            if (productId.HasValue)
            {
                return await _context.Products.AnyAsync(x => x.Id != productId && x.Slug == slug);
            }
            return await _context.Products.AnyAsync(x => x.Slug == slug);
        }

        public async Task<Product?> GetProductById(long productId)
        {
            return await _context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == productId);
        }
    }
}