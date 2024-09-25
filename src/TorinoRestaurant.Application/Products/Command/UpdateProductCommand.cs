using Microsoft.AspNetCore.Http;
using TorinoRestaurant.Application.Abstractions.Commands;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.Abstractions.Services;
using TorinoRestaurant.Core.Abstractions.Guards;
using TblCategory = TorinoRestaurant.Core.Products.Entities.Category;
using TorinoRestaurant.Application.Commons;
using TorinoRestaurant.Core.Abstractions.Exceptions;
using TorinoRestaurant.Application.Products.Models;
using TorinoRestaurant.Core.Products.Entities;

namespace TorinoRestaurant.Application.Products.Command
{
    public sealed record UpdateProductCommand(
        long Id,
        string Name,
        string Description,
        string VietnameseDescription,
        long CategoryId,
        List<ProductPriceEntity> ProductPrices,
        double CostPrice,
        bool IsUseForPrinter,
        bool IsDeleteImage,
        IFormFile? Image,
        List<long> DeletedProductPrices) : CreateCommand<long> { }

    public sealed class UpdateProductCommandHandler(IProductRepository productRepository, IRepository<TblCategory, long> categoryRepository, IUnitOfWork unitOfWork, IFileStorageService fileStorageService) : CommandHandler<UpdateProductCommand, long>(unitOfWork, fileStorageService)
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IRepository<TblCategory, long> _categoryRepository = categoryRepository;

        protected override async Task<long> HandleAsync(UpdateProductCommand request)
        {
            var existProduct = await _productRepository.GetProductById(request.Id);
            existProduct = Guard.Against.NotFound(existProduct);

            if (request.CategoryId != existProduct.CategoryId)
            {
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
                Guard.Against.NotFound(category);
                existProduct.SetCategoryId(request.CategoryId);
            }

            if (request.Name != existProduct.Name)
            {
                var slug = Helpers.ConvertToUnSign(existProduct.Name);
                var isExistSlug = await _productRepository.IsExistSlug(existProduct.Id, slug);
                if (isExistSlug)
                {
                    throw new DomainException($"There is a product exists with slug: {slug}");
                }
                existProduct.SetSlug(slug);
            }

            if (request.IsDeleteImage)
            {
                await FileStorageService.DeleteFileUrlAsync([existProduct.ImageUrl]);
                existProduct.ImageUrl = "";
                if (request.Image != null)
                {
                    using var stream = new MemoryStream();
                    await request.Image.CopyToAsync(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    var imageUrl = await FileStorageService.UploadFileGetUrlAsync(request.Image.FileName.Replace(" ", "-"), stream);
                    existProduct.ImageUrl = imageUrl;
                }
            }

            existProduct.Name = request.Name;
            existProduct.Description = request.Description;
            existProduct.VietnameseDescription = request.VietnameseDescription;
            existProduct.CostPrice = request.CostPrice;

            if (request.DeletedProductPrices.Count > 0)
            {
                await _productRepository.DeleteProductPrices(request.DeletedProductPrices);
            }

            var editProductPrices = request.ProductPrices.Where(x => x.Id.HasValue);
            foreach (var productPrice in editProductPrices)
            {
                var existProductPrice = existProduct.ProductPrices.FirstOrDefault(x => x.Id == productPrice.Id);
                if (existProductPrice != null)
                {
                    existProductPrice.Name = productPrice.Name;
                    existProductPrice.Price = productPrice.Price;
                }
            }

            var newProductPrices = request.ProductPrices.Where(x => !x.Id.HasValue);
            foreach (var productPrice in newProductPrices)
            {
                existProduct.ProductPrices.Add(new ProductPrice
                {
                    Name = productPrice.Name,
                    Price = productPrice.Price
                });
            }

            await UnitOfWork.CommitAsync();
            return request.Id;
        }
    }
}