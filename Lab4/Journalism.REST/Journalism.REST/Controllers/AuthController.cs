using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Journalism.Infrastructure.Models;
using Journalism.REST.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Journalism.REST.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _config;

		public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config)
		{
			_userManager = userManager;
			_config = config;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			// 1. шукаємо користувача
			var user = await _userManager.FindByNameAsync(request.UserName);
			if (user == null)
				return Unauthorized("Невірний логін або пароль");

			// 2. перевіряємо пароль
			var passwordOk = await _userManager.CheckPasswordAsync(user, request.Password);
			if (!passwordOk)
				return Unauthorized("Невірний логін або пароль");

			// 3. беремо першу роль (Reader / Journalist / Editor)
			var roles = await _userManager.GetRolesAsync(user);
			var role = roles.FirstOrDefault() ?? "Reader";

			// 4. формуємо claims
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name, user.UserName ?? ""),
				new Claim(ClaimTypes.Role, role)
			};

			// 5. читаємо налаштування JWT
			var jwtSection = _config.GetSection("Jwt");
			var keyBytes = Encoding.UTF8.GetBytes(jwtSection["Key"]!);
			var key = new SymmetricSecurityKey(keyBytes);

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: jwtSection["Issuer"],
				audience: jwtSection["Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(
					int.TryParse(jwtSection["ExpiresMinutes"], out var m) ? m : 60),
				signingCredentials: creds);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			// 6. віддаємо відповідь
			var response = new LoginResponse
			{
				UserName = user.UserName ?? "",
				Role = role,
				Token = tokenString
			};

			return Ok(response);
		}
	}
}
