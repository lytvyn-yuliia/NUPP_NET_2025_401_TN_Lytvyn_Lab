using System.ComponentModel.DataAnnotations;

namespace Journalism.REST.Models
{
	public class ArticleCreateRequest
	{
		[Required]
		public string Title { get; set; } = null!;

		[Required]
		public string Category { get; set; } = null!;

		[Required]
		public int JournalistId { get; set; }
	}
}
