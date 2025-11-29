namespace Journalism.RestApi.Models
{
    public class ArticleModel
    {
        public int Id { get; set; }            // Ідентифікатор статті
        public string Title { get; set; } = ""; // Заголовок
        public string Category { get; set; } = ""; // Категорія
        public int JournalistId { get; set; }      // Автор (журналіст)
    }
}
