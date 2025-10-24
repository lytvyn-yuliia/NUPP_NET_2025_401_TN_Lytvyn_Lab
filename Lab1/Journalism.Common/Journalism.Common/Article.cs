using System;

namespace Journalism.Common
{
    public class Article
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime PublishDate { get; set; }

        public Article(string title, string category)
        {
            Title = title;
            Category = category;
            PublishDate = DateTime.Now;
        }
    }
}
