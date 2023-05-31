using StudentHelper.Model.Models.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Data.Repository
{
    public interface ICourseRepository<T> where T : class
    {
        Task<Course> GetCourseByIdAsync(int courseId);
        Task UpdateAsync(T entity);
    }
}
