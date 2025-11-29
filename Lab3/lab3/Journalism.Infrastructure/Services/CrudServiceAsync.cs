using System.Collections.Generic;
using System.Threading.Tasks;
using Journalism.Infrastructure.Repositories;

namespace Journalism.Infrastructure.Services
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public CrudServiceAsync(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateAsync(T element)
        {
            await _repository.AddAsync(element);
            return true;
        }

        public async Task<T> ReadAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<bool> UpdateAsync(T element)
        {
            await _repository.UpdateAsync(element);
            return true;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            await _repository.DeleteAsync(element);
            return true;
        }

        public async Task<bool> SaveAsync()
        {
            // Цей метод зараз не обов’язковий, бо SaveChanges виконується у Repository
            return await Task.FromResult(true);
        }
    }
}
