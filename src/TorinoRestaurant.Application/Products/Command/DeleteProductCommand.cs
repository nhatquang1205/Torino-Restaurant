using MediatR;
using Microsoft.EntityFrameworkCore;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Core.Abstractions.Exceptions;

namespace TorinoRestaurant.Application.Products.Command
{
    public sealed record DeleteProductCommand(List<long> Ids) : IRequest;

    public sealed class DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productQuery = _productRepository.GetAll();
            var products = await productQuery
                .Where(x => request.Ids.Contains(x.Id)).ToListAsync(cancellationToken: cancellationToken);
            if (products.Count != request.Ids.Count)
            {
                throw new NotFoundException("products not found");
            }

            _productRepository.Remove(products);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}