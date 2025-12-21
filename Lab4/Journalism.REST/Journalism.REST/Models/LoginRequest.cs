namespace Journalism.REST.Models
{
	public class LoginRequest
	{
		public string UserName { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}
