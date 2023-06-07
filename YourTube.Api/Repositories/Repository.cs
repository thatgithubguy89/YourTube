using YourTube.Api.Data;
using Microsoft.EntityFrameworkCore;
using YourTube.Api.Interfaces;

namespace YourTube.Api.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly YourTubeContext _context;

        public Repository(YourTubeContext context)
        {
            _context = context;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var result = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return null;

            _context.Entry<T>(entity).State = EntityState.Detached;

            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
