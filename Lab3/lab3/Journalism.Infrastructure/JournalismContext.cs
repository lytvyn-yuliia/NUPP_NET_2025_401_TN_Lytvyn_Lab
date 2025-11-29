using Journalism.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Journalism.Infrastructure
{
    public class JournalismContext : DbContext
    {
        public DbSet<JournalistModel> Journalists { get; set; }
        public DbSet<ArticleModel> Articles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=C:\\Users\\user\\Desktop\\NUPP_NET_2025_401_TN_Lytvyn_Lab\\NUPP_NET_2025_401_TN_Lytvyn_Lab\\Lab3\\lab3\\Journalism.ConsoleApp\\journalism.db");
            }
        }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JournalistModel>()
                .HasKey(j => j.Id);
            modelBuilder.Entity<ArticleModel>()
                .HasKey(a => a.Id);

            // Один журналіст може мати багато статей
            modelBuilder.Entity<ArticleModel>()
                .HasOne(a => a.Journalist)
                .WithMany(j => j.Articles)
                .HasForeignKey(a => a.JournalistId);
        }
    }
}
