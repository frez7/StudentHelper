
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.SellerEntities;

namespace StudentHelper.Model.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task RemoveAsync(T entity);
        Task<T> GetByUserId(int userId);
        Task<T> FindManyToMany(int firstId, int secondId);
    }
}
