using Microsoft.EntityFrameworkCore;
using ValeraProject.Models;

namespace ValeraProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Valera> Valeras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Valera>().HasData(
                new Valera 
                { 
                    Id = 1, 
                    Health = 100, 
                    Mana = 0, 
                    Cheerfulness = 0, 
                    Fatigue = 0, 
                    Money = 100 
                }
            );
        }
    }
}