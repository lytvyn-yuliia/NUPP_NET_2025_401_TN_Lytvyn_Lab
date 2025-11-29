using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Journalism.RestApi.Models;

namespace Journalism.RestApi.Services
{
    public class ArticleInMemoryService : ICrudServiceAsync<ArticleModel>
    {
        // Поки що просто список у памʼяті
        private readonly List<ArticleModel> _articles = new();
        private int _nextId = 1;

        public Task<bool> CreateAsync(ArticleModel element)
        {
            element.Id = _nextId++;
            _articles.Add(element);
            return Task.FromResult(true);
        }

        public Task<ArticleModel?> ReadAsync(int id)
        {
            var article = _articles.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(article);
        }

        public Task<IEnumerable<ArticleModel>> ReadAllAsync()
        {
            return Task.FromResult<IEnumerable<ArticleModel>>(_articles);
        }

        public Task<IEnumerable<ArticleModel>> ReadAllAsync(int page, int amount)
        {
            var items = _articles
                .Skip((page - 1) * amount)
                .Take(amount);
            return Task.FromResult<IEnumerable<ArticleModel>>(items);
        }

        public Task<bool> UpdateAsync(ArticleModel element)
        {
            var existing = _articles.FirstOrDefault(a => a.Id == element.Id);
            if (existing == null) return Task.FromResult(false);

            existing.Title = element.Title;
            existing.Category = element.Category;
            existing.JournalistId = element.JournalistId;

            return Task.FromResult(true);
        }

        public Task<bool> RemoveAsync(ArticleModel element)
        {
            var removed = _articles.RemoveAll(a => a.Id == element.Id) > 0;
            return Task.FromResult(removed);
        }

        public Task<bool> SaveAsync()
        {
            // Для ін-меморі нічого зберігати не треба
            return Task.FromResult(true);
        }
    }
}
