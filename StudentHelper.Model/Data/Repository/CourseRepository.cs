using Microsoft.EntityFrameworkCore;
using StudentHelper.Model.Models.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Data.Repository
{
    public class CourseRepository<T> : ICourseRepository<T> where T : class
    {
        private readonly CourseContext _context;
        private readonly DbSet<T> _dbSet;

        public CourseRepository(CourseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            return course;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

    }
}
