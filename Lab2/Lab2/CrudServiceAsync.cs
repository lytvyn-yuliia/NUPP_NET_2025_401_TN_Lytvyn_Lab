using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : IIdentifiable
{
    private readonly ConcurrentDictionary<Guid, T> _store = new ConcurrentDictionary<Guid, T>();
    private readonly string _filePath;
    private readonly object _fileLock = new object();
    private readonly SemaphoreSlim _saveSemaphore = new SemaphoreSlim(1, 1);

    public CrudServiceAsync(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<bool> CreateAsync(T element)
    {
        if (element == null) return false;

        var added = _store.TryAdd(element.Id, element);
        return await Task.FromResult(added);
    }

    public async Task<T> ReadAsync(Guid id)
    {
        _store.TryGetValue(id, out var value);
        return await Task.FromResult(value);
    }

    public async Task<IEnumerable<T>> ReadAllAsync()
    {
        return await Task.FromResult(_store.Values.ToList().AsReadOnly());
    }

    public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
    {
        if (page < 1) page = 1;
        if (amount < 1) amount = 10;

        var skip = (page - 1) * amount;
        var pageItems = _store.Values.Skip(skip).Take(amount).ToList();
        return await Task.FromResult(pageItems);
    }

    public async Task<bool> UpdateAsync(T element)
    {
        if (element == null) return false;

        var updated = _store.AddOrUpdate(element.Id, element, (_, __) => element);
        return await Task.FromResult(updated != null);
    }

    public async Task<bool> RemoveAsync(T element)
    {
        if (element == null) return false;

        var removed = _store.TryRemove(element.Id, out _);
        return await Task.FromResult(removed);
    }

    public async Task<bool> SaveAsync()
    {
        await _saveSemaphore.WaitAsync();
        try
        {
           
            var list = _store.Values.ToList();

         
            var json = JsonConvert.SerializeObject(list, Formatting.Indented);

      
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

           
            lock (_fileLock)
            {
                File.WriteAllText(_filePath, json);
            }

            return true;
        }
        catch
        {
            
            return false;
        }
        finally
        {
            _saveSemaphore.Release();
        }
    }

    
    public IEnumerator<T> GetEnumerator() => _store.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
