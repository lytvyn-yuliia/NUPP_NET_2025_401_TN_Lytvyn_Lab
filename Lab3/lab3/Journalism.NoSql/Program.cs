using System;
using System.Text;
using System.Threading.Tasks;
using Journalism.NoSql.Models;
using Journalism.NoSql.Repositories;

namespace Journalism.NoSql
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // 1. Строка підключення до локального MongoDB
            string connectionString = "mongodb://localhost:27017";

            // 2. Назва бази даних
            string databaseName = "JournalismLocalNoSql";

            // 3. Створюємо репозиторій для ArticleModel, колекція "articles"
            var articleRepository = new MongoRepository<ArticleModel>(
                connectionString,
                databaseName,
                "articles");

            // 4. Створюємо нову статтю
            var article = new ArticleModel
            {
                Title = "Перша стаття",
                Category = "Culture",
                Journalist = "Yuliia Lytvyn"
                // Id НЕ задаємо – MongoDB сама створить ObjectId
            };

            await articleRepository.AddAsync(article);
            Console.WriteLine("Статтю збережено у MongoDB!");

            // 5. Зчитуємо всі статті
            var allArticles = await articleRepository.GetAllAsync();

            Console.WriteLine("\nВсі статті з MongoDB:");
            foreach (var a in allArticles)
            {
                Console.WriteLine($"{a.Id} | {a.Title} | {a.Category} | {a.Journalist}");
            }

            Console.WriteLine("\nГотово. Натисніть Enter для виходу.");
            Console.ReadLine();
        }
    }
}
