using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Journalism.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Journalism.Infrastructure
{
	public class JournalismContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<JournalistModel> Journalists { get; set; } = null!;
		public DbSet<ArticleModel> Articles { get; set; } = null!;

		public JournalismContext()
		{
		}

		public JournalismContext(DbContextOptions<JournalismContext> options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlite(
					"Data Source=C:\\Users\\user\\Desktop\\NUPP_NET_2025_401_TN_Lytvyn_Lab\\NUPP_NET_2025_401_TN_Lytvyn_Lab\\Lab3\\lab3\\Journalism.ConsoleApp\\journalism.db");
			}
		}


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Ключі для статей/журналістів
			builder.Entity<JournalistModel>().HasKey(j => j.Id);
			builder.Entity<ArticleModel>().HasKey(a => a.Id);

			builder.Entity<ArticleModel>()
				.HasOne(a => a.Journalist)
				.WithMany(j => j.Articles)
				.HasForeignKey(a => a.JournalistId);

			// ==== SEED Identity ролей та користувачів ====

			var readerRoleId = "role-reader";
			var journalistRoleId = "role-journalist";
			var editorRoleId = "role-editor";

			builder.Entity<IdentityRole>().HasData(
				new IdentityRole { Id = readerRoleId, Name = "Reader", NormalizedName = "READER" },
				new IdentityRole { Id = journalistRoleId, Name = "Journalist", NormalizedName = "JOURNALIST" },
				new IdentityRole { Id = editorRoleId, Name = "Editor", NormalizedName = "EDITOR" }
			);

			var readerId = "user-reader";
			var journalistId = "user-journalist";
			var editorId = "user-editor";

			var hasher = new PasswordHasher<ApplicationUser>();

			var reader = new ApplicationUser
			{
				Id = readerId,
				UserName = "reader",
				NormalizedUserName = "READER",
				Email = "reader@test.com",
				NormalizedEmail = "READER@TEST.COM",
				EmailConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString()
			};
			reader.PasswordHash = hasher.HashPassword(reader, "reader123");

			var journalist = new ApplicationUser
			{
				Id = journalistId,
				UserName = "journalist",
				NormalizedUserName = "JOURNALIST",
				Email = "journalist@test.com",
				NormalizedEmail = "JOURNALIST@TEST.COM",
				EmailConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString()
			};
			journalist.PasswordHash = hasher.HashPassword(journalist, "journalist123");

			var editor = new ApplicationUser
			{
				Id = editorId,
				UserName = "editor",
				NormalizedUserName = "EDITOR",
				Email = "editor@test.com",
				NormalizedEmail = "EDITOR@TEST.COM",
				EmailConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString()
			};
			editor.PasswordHash = hasher.HashPassword(editor, "editor123");

			builder.Entity<ApplicationUser>().HasData(reader, journalist, editor);

			builder.Entity<IdentityUserRole<string>>().HasData(
				new IdentityUserRole<string> { UserId = readerId, RoleId = readerRoleId },
				new IdentityUserRole<string> { UserId = journalistId, RoleId = journalistRoleId },
				new IdentityUserRole<string> { UserId = editorId, RoleId = editorRoleId }
			);
		}

	}
}
