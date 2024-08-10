using Microsoft.EntityFrameworkCore;

namespace PetPlayApp.Server.Db
{
    public class DatabaseContext : DbContext
    {
        // DbSet for users table
        public DbSet<User> Users { get; set; }
        // DbSet for posts table
        public DbSet<Post> Posts { get; set; }
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
    }
}
