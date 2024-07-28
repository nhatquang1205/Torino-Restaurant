using Microsoft.EntityFrameworkCore;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Core.Abstractions.Entities;

namespace TorinoRestaurant.Infrastructure.Repositories
{
    internal class Repository<T, P> : IRepository<T, P> where T : AggregateRoot<P>
    {
        protected readonly DataContext _context;
        protected readonly DbSet<T> _entitySet;

        public Repository(DataContext context)
        {
            _context = context;
            _entitySet = _context.Set<T>();
        }

        public IQueryable<T> GetAll(bool noTracking = true)
        {
            var set = _entitySet;
            if (noTracking)
            {
                return set.AsNoTracking();
            }
            return set;
        }

        public async Task<T?> GetByIdAsync(P id)
        {
            return await _entitySet.FindAsync(id);
        }

        public async Task Insert(T entity)
        {
            await _entitySet.AddAsync(entity);
        }

        public async Task Insert(List<T> entities)
        {
            await _entitySet.AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
            _entitySet.Remove(entity);
        }

        public void Remove(IEnumerable<T> entitiesToRemove)
        {
            _entitySet.RemoveRange(entitiesToRemove);
        }
    }
}