using Journalism.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("JournalismConnection");

builder.Services.AddDbContext<JournalismContext>(options =>
	options.UseSqlite(connectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute(); // MVC


app.Run();
