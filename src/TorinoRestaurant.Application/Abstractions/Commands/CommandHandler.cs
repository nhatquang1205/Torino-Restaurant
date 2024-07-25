using MediatR;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.Abstractions.Services;

namespace TorinoRestaurant.Application.Abstractions.Commands
{
    public abstract class CommandHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
    {
        protected readonly IUnitOfWork UnitOfWork = unitOfWork;
        protected readonly IFileStorageService FileStorageService = fileStorageService;
    }

    public abstract class CommandHandler<TCommand, P>(IUnitOfWork unitOfWork, IFileStorageService fileStorageService) : CommandHandler(unitOfWork, fileStorageService), IRequestHandler<TCommand, P> where TCommand: CreateCommand<P>
    {
        protected abstract Task<P> HandleAsync(TCommand request);

        public async Task<P> Handle(TCommand request, CancellationToken cancellationToken)
        {
            return await HandleAsync(request);
        }
    }
}