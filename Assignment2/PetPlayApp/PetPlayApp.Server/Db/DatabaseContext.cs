using Microsoft.EntityFrameworkCore;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Db
{
    public class DatabaseContext : DbContext
    {
        // DbSet for users table
        public DbSet<User> Users { get; set; }
        // DbSet for posts table
        public DbSet<Post> Posts { get; set; }
        // DbSet for matches table
        public DbSet<Match> Matches { get; set; }
        // DbSet for stories table
        public DbSet<Story> Stories { get; set; }
        
        public string DbPath { get; }

        public DatabaseContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "PetPlay.db");
            Console.WriteLine("Database path:" + DbPath);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configures the context to use SQLite as the database provider
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Post>()
				.HasMany(e => e.Likes)
				.WithMany(e => e.LikedPosts)
				.UsingEntity<Like>();

            modelBuilder.Entity<Post>()
                .HasOne(e => e.PostCreator)
                .WithMany(e => e.CreatedPosts);

			modelBuilder.Entity<Match>()
				.HasKey(m => new { m.User1Id, m.User2Id });

			modelBuilder.Entity<Match>()
				.HasOne(m => m.User1)
				.WithMany(u => u.MatchesInitiated)
				.HasForeignKey(m => m.User1Id)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Match>()
				.HasOne(m => m.User2)
				.WithMany(u => u.MatchesReceived)
				.HasForeignKey(m => m.User2Id)
				.OnDelete(DeleteBehavior.Restrict);
			
			modelBuilder.Entity<Story>()
				.HasOne(s => s.StoryCreator)
				.WithMany(u => u.StoriesCreated)
				.HasForeignKey(s => s.StoryCreatorId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
