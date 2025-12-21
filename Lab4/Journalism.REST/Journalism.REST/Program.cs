using System.Text;
using Journalism.Infrastructure;
using Journalism.Infrastructure.Models;
using Journalism.Infrastructure.Repositories;
using Journalism.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ======================= DbContext =======================
builder.Services.AddDbContext<JournalismContext>(options =>
	options.UseSqlite(builder.Configuration.GetConnectionString("JournalismConnection")));

// ======================= Identity ========================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<JournalismContext>()
	.AddDefaultTokenProviders();

// ============== Наші репозиторії / сервіси ===============
// generic-репозиторій для всіх моделей (ArticleModel, JournalistModel тощо)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// CRUD-сервіс для всіх моделей
builder.Services.AddScoped(typeof(ICrudServiceAsync<>), typeof(CrudServiceAsync<>));

// ========================= JWT ===========================
var jwtSection = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwtSection["Key"]!); // Key має бути довгим (32+ символи)

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtSection["Issuer"],
		ValidAudience = jwtSection["Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
	};
});

// ======================= MVC + Swagger ===================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ЄДИНИЙ виклик AddSwaggerGen – тут же додаємо JWT
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Journalism.REST",
		Version = "v1"
	});

	// Опис схеми безпеки Bearer
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Введіть токен так: Bearer {your token}"
	});

	// Вимога використовувати цю схему за замовчуванням
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

var app = builder.Build();

// ====================== HTTP-конвеєр =====================
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
