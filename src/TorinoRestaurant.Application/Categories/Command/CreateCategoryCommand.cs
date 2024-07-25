using Microsoft.AspNetCore.Http;
using TorinoRestaurant.Application.Abstractions.Commands;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.Abstractions.Services;
using TblCategory = TorinoRestaurant.Core.Products.Entities.Category;
namespace TorinoRestaurant.Application.Categories.Command
{
    public sealed record CreateCategoryCommand(string Name, string Description, IFormFile Image) : CreateCommand<long> { }

    public sealed class CreateCategoryCommandHandler(IRepository<TblCategory, long> categoriesRepository, IUnitOfWork unitOfWork, IFileStorageService fileStorageService) : CommandHandler<CreateCategoryCommand, long>(unitOfWork, fileStorageService)
    {
        private readonly IRepository<TblCategory, long> _categoriesRepository = categoriesRepository;

        protected override async Task<long> HandleAsync(CreateCategoryCommand request)
        {
            var imageUrl = "";
            if (request.Image != null)  
            {
                using var stream = new MemoryStream();
                await request.Image.CopyToAsync(stream);
                stream.Position = 0;
                stream.Seek(0, SeekOrigin.Begin);
                imageUrl = await FileStorageService.UploadFileGetUrlAsync(request.Image.FileName.Replace(" ", "-"), stream);
            }
            var category = TblCategory.Create(request.Name, request.Description, imageUrl);
            await _categoriesRepository.Insert(category);

            await UnitOfWork.CommitAsync();
            return category.Id;
        }
    }
}