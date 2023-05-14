
using Microsoft.EntityFrameworkCore;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.SellerEntities;

namespace StudentHelper.Model.Data.Repository
{
    public class Repository<T> : IRepository<T> where T: class
    {
        private readonly CourseContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(CourseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
            T result = _dbSet.Add(entity).Entity;
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<SellerApplication> GetByUserId(int userId)
        {
            return await _context.SellerApplications.FirstOrDefaultAsync(s => s.UserId == userId);
        }
    }
}
