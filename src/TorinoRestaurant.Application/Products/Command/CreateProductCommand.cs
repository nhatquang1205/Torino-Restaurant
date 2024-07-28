using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TorinoRestaurant.Application.Abstractions.Commands;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.Abstractions.Services;
using TblProduct = TorinoRestaurant.Core.Products.Entities.Product;
using TblCategory = TorinoRestaurant.Core.Products.Entities.Category;
using TorinoRestaurant.Core.Abstractions.Guards;
using TorinoRestaurant.Application.Commons;
using TorinoRestaurant.Core.Abstractions.Exceptions;
using TorinoRestaurant.Core.Products.Entities;


namespace TorinoRestaurant.Application.Products.Command
{
    public sealed record CreateProductCommand(
        string Name,
        string Description,
        string VietnameseDescription,
        long CategoryId,
        double Price,
        double CostPrice,
        bool IsUseForPrinter,
        bool IsDeleteImage,
        IFormFile? Image) : CreateCommand<long> { }

    public class CreateProductCommandHandler(
        IProductRepository productsRepository,
        IRepository<TblCategory, long> categoryRepository,
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService) : CommandHandler<CreateProductCommand, long>(unitOfWork, fileStorageService)
    {
        private readonly IProductRepository _productsRepository = productsRepository;
        private readonly IRepository<TblCategory, long> _categoryRepository = categoryRepository;

        protected override async Task<long> HandleAsync(CreateProductCommand request)
        {
            var product = TblProduct.Create(request.Name, request.Description, request.VietnameseDescription, request.Price, request.CostPrice, request.IsUseForPrinter);
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            Guard.Against.NotFound(category);
            product.SetCategoryId(request.CategoryId);

            var slug = Helpers.ConvertToUnSign(product.Name);
            var isExistSlug = await _productsRepository.IsExistSlug(null, slug);
            if (isExistSlug)
            {
                throw new DomainException($"There is a product exists with slug: {slug}");
            }
            product.SetSlug(slug);

            if (request.Image != null)  
            {
                using var stream = new MemoryStream();
                await request.Image.CopyToAsync(stream);
                stream.Position = 0;
                stream.Seek(0, SeekOrigin.Begin);
                var imageUrl = await FileStorageService.UploadFileGetUrlAsync($"{TblProduct.FolderImagePrefix}/{request.Image.FileName.Replace(" ", "-")}", stream);
                product.SetImageUrl(imageUrl);
            }

            await _productsRepository.Insert(product);

            await UnitOfWork.CommitAsync();
            return product.Id;
        }
    }
}