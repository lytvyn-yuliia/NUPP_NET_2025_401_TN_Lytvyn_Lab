using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace Journalism.Common
{
    public class CrudService<T> : ICrudService<T> where T : class
    {
        private readonly List<T> _items = new List<T>();

        public void Create(T element) => _items.Add(element);
        public T Read(Guid id)
        {
            return _items.FirstOrDefault(x =>
                x?.GetType().GetProperty("Id")?.GetValue(x)?.Equals(id) == true);
        }
        public IEnumerable<T> ReadAll() => _items;
        public void Update(T element)
        {
            var id = element?.GetType().GetProperty("Id")?.GetValue(element);
            var old = Read((Guid)id);
            if (old != null)
            {
                _items.Remove(old);
                _items.Add(element);
            }
        }
        public void Remove(T element) => _items.Remove(element);


        public void Save(string filePath)
        {
            var json = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        // Метод завантаження з файлу
        public void Load(string filePath)
        {
            if (!File.Exists(filePath)) return;

            var json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<List<T>>(json);
            if (data != null)
            {
                _items.Clear();
                _items.AddRange(data);
            }
        }

    }
}
