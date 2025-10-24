using System;


namespace Journalism.Common
{
    public class Person
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int Experience { get; set; }
        public string City { get; set; }

        // Конструктор
        public Person(string name, int experience, string city)
        {
            Name = name;
            Experience = experience;
            City = city;
        }
    }
}

