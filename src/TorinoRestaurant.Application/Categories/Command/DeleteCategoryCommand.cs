using MediatR;
using Microsoft.EntityFrameworkCore;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Core.Abstractions.Exceptions;
using TblCategory = TorinoRestaurant.Core.Products.Entities.Category;

namespace TorinoRestaurant.Application.Categories.Command
{
    public sealed record DeleteCategoryCommand(List<long> Ids) : IRequest;

    public sealed class DeleteCategoryCommandHandler(IRepository<TblCategory, long> categoriesRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IRepository<TblCategory, long> _categoriesRepository = categoriesRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryQuery = _categoriesRepository.GetAll();
            var categories = await categoryQuery
                .Include(x => x.Products)
                .Where(x => request.Ids.Contains(x.Id)).ToListAsync(cancellationToken: cancellationToken);
            if (categories.Count != request.Ids.Count)
            {
                throw new NotFoundException("Categories not found");
            }
            var isHaveProductCategory = categories.Find(x => x.Products.Count != 0);
            if (isHaveProductCategory != null)
            {
                throw new DomainException("Can not delete category that has products");
            }
            
            _categoriesRepository.Remove(categories);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}