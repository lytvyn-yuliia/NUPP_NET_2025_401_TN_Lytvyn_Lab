using System;
using Microsoft.Extensions.DependencyInjection;
using Journalism.Infrastructure;
using Journalism.Infrastructure.Models;
using Journalism.Infrastructure.Repositories;
using Journalism.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;     // для Database.EnsureCreated()



namespace Journalism.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            //1. Налаштування контейнера залежностей
            var services = new ServiceCollection();

            services.AddDbContext<JournalismContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(ICrudServiceAsync<>), typeof(CrudServiceAsync<>));

            var provider = services.BuildServiceProvider();
            // Створюємо базу, якщо її немає
            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<JournalismContext>();
                db.Database.EnsureCreated();   // команда створить БД і таблиці
            }

            // Отримуємо сервіси
            var journalistService = provider.GetService<ICrudServiceAsync<JournalistModel>>();
            var articleService = provider.GetService<ICrudServiceAsync<ArticleModel>>();

            var journalist = new JournalistModel
            {
                Name = "Yuliia Lytvyn",
                Experience = 3,
                City = "Horishni Plavni"
            };

            // Зберігаємо журналіста
            await journalistService.CreateAsync(journalist);

           
            var article = new ArticleModel
            {
                Title = "Autumn in the City",
                Category = "Culture",
                JournalistId = journalist.Id
            };

            // Зберігаємо статтю
            await articleService.CreateAsync(article);


            // Зчитуємо дані з бази
            Console.WriteLine("\nЖурналісти:");
            var journalists = await journalistService.ReadAllAsync();
            foreach (var j in journalists)
                Console.WriteLine($"{j.Name}, {j.City}");

            Console.WriteLine("\nСтатті:");
            var articles = await articleService.ReadAllAsync();
            foreach (var a in articles)
                Console.WriteLine($"{a.Title}, категорія: {a.Category}");

            Console.WriteLine("\nДані успішно додано до бази journalism.db!");
            Console.ReadLine();
        }
    }
}
