using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journalism.NoSql.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T> GetByIdAsync(string id);       
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task RemoveAsync(string id);
    }
}
