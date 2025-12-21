namespace Journalism.Infrastructure.Models
{
	public class EditorProfile
	{
		public int Id { get; set; }

	
		public int UserId { get; set; }

		public string FullName { get; set; } = null!;

		// Права редактора
		public bool CanApproveArticles { get; set; } = true;
		public bool CanManageJournalists { get; set; } = true;
	}
}
