using Microsoft.AspNetCore.Http;
using TorinoRestaurant.Application.Abstractions.Commands;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.Abstractions.Services;
using TorinoRestaurant.Core.Abstractions.Guards;
using TblCategory = TorinoRestaurant.Core.Products.Entities.Category;

namespace TorinoRestaurant.Application.Categories.Command
{
    public sealed record UpdateCategoryCommand(long Id, string Name, string Description, IFormFile? Image, bool IsDeleteImage) : CreateCommand<long> { }

    public sealed class UpdateCategoryCommandHandler(IRepository<TblCategory, long> categoriesRepository, IUnitOfWork unitOfWork, IFileStorageService fileStorageService) : CommandHandler<UpdateCategoryCommand, long>(unitOfWork, fileStorageService)
    {
        private readonly IRepository<TblCategory, long> _categoriesRepository = categoriesRepository;

        protected override async Task<long> HandleAsync(UpdateCategoryCommand request)
        {
            var existCategory = await _categoriesRepository.GetByIdAsync(request.Id);
            existCategory = Guard.Against.NotFound(existCategory);
            if (request.IsDeleteImage)  
            {
                await FileStorageService.DeleteFileUrlAsync([existCategory.ImageUrl]);
                existCategory.ImageUrl = "";
                if (request.Image != null)
                {
                    using var stream = new MemoryStream();
                    await request.Image.CopyToAsync(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    var imageUrl = await FileStorageService.UploadFileGetUrlAsync(request.Image.FileName.Replace(" ", "-"), stream);
                    existCategory.ImageUrl = imageUrl;
                }
            }

            existCategory.Name = request.Name;
            existCategory.Description = request.Description;

            await UnitOfWork.CommitAsync();
            return request.Id;
        }
    }
}