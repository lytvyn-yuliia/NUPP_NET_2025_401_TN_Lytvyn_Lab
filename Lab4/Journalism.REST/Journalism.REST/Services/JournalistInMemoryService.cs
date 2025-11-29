using Journalism.REST.Models;
using Journalism.RestApi.Services;

namespace Journalism.REST.Services
{
    public class JournalistInMemoryService : ICrudServiceAsync<JournalistModel>
    {
        private readonly List<JournalistModel> _items = new();
        private int _nextId = 1;

        public JournalistInMemoryService()
        {
            // Невеликий початковий список – щоб було що читати
            _items.AddRange(new[]
            {
                new JournalistModel
                {
                    Id = _nextId++,
                    Name = "Yuliia Lytvyn",
                    Email = "yuliia@example.com",
                    Phone = "+380683184161",
                    Specialization = "Local news",
                    ExperienceYears = 2
                },
                new JournalistModel
                {
                    Id = _nextId++,
                    Name = "Ivan Petrenko",
                    Email = "ivan@example.com",
                    Phone = "+380971234567",
                    Specialization = "Culture",
                    ExperienceYears = 5
                }
            });
        }

        public Task<bool> CreateAsync(JournalistModel element)
        {
            if (element == null) return Task.FromResult(false);

            element.Id = _nextId++;
            _items.Add(element);
            return Task.FromResult(true);
        }

        public Task<JournalistModel?> ReadAsync(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(item);
        }

        public Task<IEnumerable<JournalistModel>> ReadAllAsync()
        {
            return Task.FromResult(_items.AsEnumerable());
        }

        public Task<IEnumerable<JournalistModel>> ReadAllAsync(int page, int amount)
        {
            if (page < 1) page = 1;
            if (amount < 1) amount = 10;

            var result = _items
                .Skip((page - 1) * amount)
                .Take(amount)
                .AsEnumerable();

            return Task.FromResult(result);
        }

        public Task<bool> UpdateAsync(JournalistModel element)
        {
            if (element == null) return Task.FromResult(false);

            var existing = _items.FirstOrDefault(x => x.Id == element.Id);
            if (existing == null) return Task.FromResult(false);

            existing.Name = element.Name;
            existing.Email = element.Email;
            existing.Phone = element.Phone;
            existing.Specialization = element.Specialization;
            existing.ExperienceYears = element.ExperienceYears;

            return Task.FromResult(true);
        }

        public Task<bool> RemoveAsync(JournalistModel element)
        {
            if (element == null) return Task.FromResult(false);

            var existing = _items.FirstOrDefault(x => x.Id == element.Id);
            if (existing == null) return Task.FromResult(false);

            _items.Remove(existing);
            return Task.FromResult(true);
        }

        public Task<bool> SaveAsync()
        {
            // In-memory – зберігати нічого, просто true
            return Task.FromResult(true);
        }
    }
}
