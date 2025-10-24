using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journalism.Common;

namespace Journalism.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            //Сховища пам'яті для журналістів і статей
            var journalistService = new CrudService<Journalist>();
            var articleService = new CrudService<Article>();

            Console.WriteLine("Autumn in the City".Quote());

            var j1 = new Journalist("Yuliia Lytvyn", 3, "Horishni Plavni", "Local News");
            journalistService.Create(j1);

            var article = new Article("Autumn in the City", "Culture");
            articleService.Create(article);

            j1.PublishArticle(article.Title);

            Console.WriteLine("\nAll journalists:");
            foreach (var j in journalistService.ReadAll())
                Console.WriteLine($"{j.Name}, {j.City}");

            Console.WriteLine("\nAll articles:");
            foreach (var a in articleService.ReadAll())
                Console.WriteLine($"{a.Title}, category: {a.Category}");

            // Зберегти дані у файли
            journalistService.Save("journalists.json");
            articleService.Save("articles.json");

            // Очистити і перевірити завантаження
            journalistService.Load("journalists.json");
            articleService.Load("articles.json");


            Console.ReadLine();
        }
    }
}
