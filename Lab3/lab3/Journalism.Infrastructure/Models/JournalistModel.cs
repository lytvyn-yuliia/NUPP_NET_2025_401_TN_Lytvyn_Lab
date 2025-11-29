using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journalism.Infrastructure.Models
{
    public class JournalistModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Experience { get; set; } // 👈 додай оце поле
        public string City { get; set; } = string.Empty;

        // Зв’язок із статтями
        public ICollection<ArticleModel> Articles { get; set; } = new List<ArticleModel>();
    }
}
