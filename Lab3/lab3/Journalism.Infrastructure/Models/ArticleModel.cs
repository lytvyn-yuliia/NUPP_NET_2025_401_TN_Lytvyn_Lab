using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Journalism.Infrastructure.Models
{
	public class ArticleModel
	{
		public int Id { get; set; }   // БЕЗ [Required]

		[Required]
		public string Title { get; set; } = null!;

		[Required]
		public string Category { get; set; } = null!;

		[Required]
		public int JournalistId { get; set; }

		[JsonIgnore]
		public JournalistModel? Journalist { get; set; }
	}
}
