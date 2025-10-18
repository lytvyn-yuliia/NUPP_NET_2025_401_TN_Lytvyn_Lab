using System;

public class Bus : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Model { get; set; }
    public int Capacity { get; set; }         
    public int Mileage { get; set; }         
    public double FuelConsumption { get; set; } 
    public string RegistrationNumber { get; set; }
    public DateTime ManufacturedAt { get; set; }

    private static readonly object _rndLock = new object();
    private static readonly Random _globalRnd = new Random();

    public static Bus CreateNew()
    {
        
        int capacity;
        int mileage;
        double fuel;
        string model;
        string reg;

        lock (_rndLock)
        {
            capacity = _globalRnd.Next(12, 85); // example capacities
            mileage = _globalRnd.Next(1_000, 300_000);
            fuel = Math.Round(_globalRnd.NextDouble() * 20 + 5, 2); // 5..25 l/100km
            model = $"Model-{_globalRnd.Next(1000, 9999)}";
            reg = $"AB{_globalRnd.Next(10, 99)}-{_globalRnd.Next(100, 999)}";
        }

        return new Bus
        {
            Id = Guid.NewGuid(),
            Model = model,
            Capacity = capacity,
            Mileage = mileage,
            FuelConsumption = fuel,
            RegistrationNumber = reg,
            ManufacturedAt = DateTime.UtcNow.AddYears(-(new Random().Next(0, 20)))
        };
    }
}
