using Journalism.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Journalism.Infrastructure
{
	public class JournalismContext : DbContext
	{
		public JournalismContext(DbContextOptions<JournalismContext> options)
			: base(options)
		{
		}

		public DbSet<JournalistModel> Journalists { get; set; }
		public DbSet<ArticleModel> Articles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<JournalistModel>()
				.HasKey(j => j.Id);

			modelBuilder.Entity<ArticleModel>()
				.HasKey(a => a.Id);

			modelBuilder.Entity<ArticleModel>()
				.HasOne(a => a.Journalist)
				.WithMany(j => j.Articles)
				.HasForeignKey(a => a.JournalistId);
		}
	}
}

