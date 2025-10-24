using System;


namespace Journalism.Common
{
    public class Journalist : Person
    {
        public string Specialization { get; set; }
        public int PublishedArticles { get; set; }

        // Конструктор
        public Journalist(string name, int experience, string city, string specialization)
            : base(name, experience, city)
        {
            Specialization = specialization;
        }

        // Метод для публікації статті
        public void PublishArticle(string title)
        {
            PublishedArticles++;
            Console.WriteLine($"{Name} published article: {title}");
        }

        // Статичне поле
        public static string Profession = "Journalist";

        // Статичний метод
        public static void PrintProfession() => Console.WriteLine($"Profession: {Profession}");
    }
}

