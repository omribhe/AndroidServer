using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
namespace WebApplication1
{
    public class WebApplicationContext: DbContext
    {
        private const string connectionString = "server=localhost;port=3306;database=DBwebServer;user=root;password=gunrhcijnu1";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the Name property as the primary
            // key of the Items table
            modelBuilder.Entity<User>().HasKey(e => e.Name);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Message> Messages { get; set; }

    }
}
