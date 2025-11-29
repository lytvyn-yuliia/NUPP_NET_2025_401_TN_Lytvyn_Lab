using Journalism.REST.Models;
using Journalism.REST.Services;
using Journalism.RestApi.Models;
using Journalism.RestApi.Services;

namespace Journalism.REST
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Ðåºñòðóºìî CRUD ñåðâ³ñ (ÊÐÎÊ 7)
            builder.Services.AddSingleton<ICrudServiceAsync<ArticleModel>, ArticleInMemoryService>();
            builder.Services.AddSingleton<ICrudServiceAsync<JournalistModel>, JournalistInMemoryService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
