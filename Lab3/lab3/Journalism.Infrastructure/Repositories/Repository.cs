using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Journalism.Infrastructure.Repositories
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly JournalismContext _context;
		private readonly DbSet<T> _set;

		public Repository(JournalismContext context)
		{
			_context = context;
			_set = _context.Set<T>();
		}

		public async Task AddAsync(T entity)
		{
			await _set.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			var item = await _context.Set<T>().FindAsync(id);

			
			if (item == null)
				throw new InvalidOperationException($"Entity {typeof(T).Name} with id={id} not found.");

			return item;
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _set.ToListAsync();
		}

		public async Task UpdateAsync(T entity)
		{
			_set.Update(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			_set.Remove(entity);
			await _context.SaveChangesAsync();
		}
	}
}
