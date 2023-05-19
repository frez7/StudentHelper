
using Microsoft.EntityFrameworkCore;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.CourseEntities;
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
        public async Task<T> GetByUserId(int userId) 
        {
            if (typeof(T) == typeof(SellerApplication))
            {
                var sellerapplication = await _context.SellerApplications.FirstOrDefaultAsync(s => s.UserId == userId);
                return sellerapplication as T;
            }
            else if(typeof(T) == typeof(Seller))
            {
                var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);
                return seller as T;
            }
            else if(typeof(T) == typeof(Student))
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
                return student as T;
            }
            return null;
        }

        public async Task<T> FindManyToMany(int firstId, int secondId)
        {
            if (typeof(T) == typeof(StudentCourse))
            {
                var studentCourse = await _context.StudentCourses.FindAsync(firstId, secondId);
                return studentCourse as T;
            }
            return null;
            
        }
        public async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StudentCourse>> GetCoursesByStudentId(int studentId)
        {
            var items = _context.StudentCourses.Where(i => i.StudentId == studentId).ToList();
            return items;
        }
    }
}
