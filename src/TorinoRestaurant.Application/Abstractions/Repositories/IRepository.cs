using TorinoRestaurant.Core.Abstractions.Entities;

namespace TorinoRestaurant.Application.Abstractions.Repositories
{
    public interface IRepository<T, P> where T : AggregateRoot<P>
    {
        IQueryable<T> GetAll(bool noTracking = true);
        Task<T?> GetByIdAsync(P id);
        Task Insert(T entity);
        Task Insert(List<T> entities);
        void Delete(T entity);
        void Remove(IEnumerable<T> entitiesToRemove);
    }
}