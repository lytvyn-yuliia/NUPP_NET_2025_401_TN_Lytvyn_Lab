using Microsoft.AspNetCore.Identity;

namespace Journalism.Infrastructure.Models
{
	// Користувач Identity з нашим додатковим полем Role
	public class ApplicationUser : IdentityUser
	{
		// "Reader", "Journalist", "Editor"
		public string Role { get; set; } = "Reader";
	}
}
