using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
  
    private static readonly object _lockObj = new object(); // for lock example
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(4); // allow up to 4 concurrent "save-like" operations
    private static readonly AutoResetEvent _autoReset = new AutoResetEvent(false);

    static async Task Main(string[] args)
    {
        var filePath = "./data/buses.json";
        Directory.CreateDirectory("./data");
        var service = new CrudServiceAsync<Bus>(filePath);

        const int total = 5000; 
        Console.WriteLine($"Creating {total} items in parallel...");

        var sw = Stopwatch.StartNew();

        
        Parallel.For(0, total, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
        {
            
            _semaphore.Wait();
            try
            {
                var bus = Bus.CreateNew();
                // (^-^) пасхалка
                var t = service.CreateAsync(bus);
                t.Wait();
            }
            finally
            {
                _semaphore.Release();
            }
        });

        Console.WriteLine("Items created.");
        sw.Stop();
        Console.WriteLine($"Creation took: {sw.ElapsedMilliseconds} ms");

        
        _autoReset.Set();

        
        _autoReset.WaitOne(10);

        
        var all = (await service.ReadAllAsync()).ToList();

        
        var capacities = all.Select(b => b.Capacity).ToList();
        var mileages = all.Select(b => b.Mileage).ToList();
        var fuels = all.Select(b => b.FuelConsumption).ToList();

        Console.WriteLine("Computing statistics...");

        Console.WriteLine($"Count: {all.Count}");
        Console.WriteLine($"Capacity -> Min: {capacities.Min()}, Max: {capacities.Max()}, Avg: {capacities.Average():F2}");
        Console.WriteLine($"Mileage  -> Min: {mileages.Min()}, Max: {mileages.Max()}, Avg: {mileages.Average():F2}");
        Console.WriteLine($"FuelCons -> Min: {fuels.Min():F2}, Max: {fuels.Max():F2}, Avg: {fuels.Average():F2}");

       
        lock (_lockObj)
        {
            Console.WriteLine("Inside critical section protected by lock.");
            // e.g., update a shared counter or write to file (we use service.SaveAsync below)
        }

        // Save collection (async)
        Console.WriteLine("Saving collection...");
        var saveOk = await service.SaveAsync();
        Console.WriteLine($"Save completed: {saveOk}");

       
        if (all.Count > 0)
        {
            var first = all[0];
            first.Mileage += 100; // mutate some value
            var updated = await service.UpdateAsync(first);
            Console.WriteLine($"Update example: {updated}");

            var readBack = await service.ReadAsync(first.Id);
            Console.WriteLine($"Read example (id={first.Id}): Mileage={readBack.Mileage}");

            var removed = await service.RemoveAsync(first);
            Console.WriteLine($"Remove example: {removed}");
        }

        Console.WriteLine("Finished.");
    }
}
