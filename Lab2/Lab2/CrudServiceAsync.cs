using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : IIdentifiable
{
    private readonly ConcurrentDictionary<Guid, T> _store = new ConcurrentDictionary<Guid, T>();
    private readonly string _filePath;
    private readonly object _fileLock = new object();
    readonly SemaphoreSlim _saveSemaphore = new SemaphoreSlim(1, 1);
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { WriteIndented = true };

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
        return await Task.FromResult(_store.AddOrUpdate(element.Id, element, (k, v) => element) != null);
    }

    public async Task<bool> RemoveAsync(T element)
    {
        if (element == null) return false;
        return await Task.FromResult(_store.TryRemove(element.Id, out _));
    }

    public async Task<bool> SaveAsync()
    {
        // Use SemaphoreSlim to limit concurrent save operations
        await _saveSemaphore.WaitAsync();
        try
        {
            // serialize to JSON asynchronously
            var list = _store.Values.ToList();
            var json = JsonSerializer.Serialize(list, _jsonOptions);

            // Use lock to make actual file write thread-safe
            lock (_fileLock)
            {
                // Use File.WriteAllTextAsync to write asynchronously
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

    // IEnumerable<T> implementation
    public IEnumerator<T> GetEnumerator() => _store.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
