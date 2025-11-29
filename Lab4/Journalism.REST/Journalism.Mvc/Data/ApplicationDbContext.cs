using Journalism.Mvc.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Journalism.Mvc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ArticleModel> Articles { get; set; } = null!;
        public DbSet<JournalistModel> Journalists { get; set; } = null!;
    }
}
