using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PetPlayApp.Server.Db
{
    public class DatabaseContext : DbContext
    {
        // DbSet for books table
        public DbSet<User> Users { get; set; }
        // DbSet for authors table
        public DbSet<Post> Posts { get; set; }
        public string DbPath { get; }

        public DatabaseContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "PetPlay.db");
            Console.WriteLine("Database path:" + DbPath);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configures the context to use SQLite as the database provider
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }
}
