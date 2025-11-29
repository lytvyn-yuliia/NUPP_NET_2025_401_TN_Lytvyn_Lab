using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journalism.Infrastructure.Services
{
    public interface ICrudServiceAsync<T> where T : class
    {
        Task<bool> CreateAsync(T element);
        Task<T> ReadAsync(int id);
        Task<IEnumerable<T>> ReadAllAsync();
        Task<bool> UpdateAsync(T element);
        Task<bool> RemoveAsync(T element);
        Task<bool> SaveAsync();
    }
}
