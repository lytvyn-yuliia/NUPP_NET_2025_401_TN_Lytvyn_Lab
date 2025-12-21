namespace Journalism.Infrastructure.Models
{
	public class JournalistProfile
	{
		public int Id { get; set; }

		// FK на UserModel
		public int UserId { get; set; }

		public string FullName { get; set; } = null!;
		public string Specialty { get; set; } = "General";
		public int ArticlesPublished { get; set; } = 0;
	}
}
