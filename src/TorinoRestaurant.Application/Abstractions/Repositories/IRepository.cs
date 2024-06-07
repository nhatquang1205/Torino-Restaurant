using TorinoRestaurant.Core.Abstractions.Entities;

namespace TorinoRestaurant.Application.Abstractions.Repositories
{
    public interface IRepository<T> where T : AggregateRoot
    {
        IQueryable<T> GetAll(bool noTracking = true);
        Task<T?> GetByIdAsync(Guid id);
        Task Insert(T entity);
        Task Insert(List<T> entities);
        void Delete(T entity);
        void Remove(IEnumerable<T> entitiesToRemove);
    }
}