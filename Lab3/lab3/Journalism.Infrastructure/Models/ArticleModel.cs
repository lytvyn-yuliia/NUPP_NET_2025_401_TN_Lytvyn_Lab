using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journalism.Infrastructure.Models
{
    public class ArticleModel
    {
        public int Id { get; set; }           // Унікальний ідентифікатор
        public string Title { get; set; }     // Назва статті
        public string Category { get; set; }  // Категорія
        public int JournalistId { get; set; } // Зовнішній ключ
        public JournalistModel Journalist { get; set; } // Зв’язок із журналістом
    }
}
