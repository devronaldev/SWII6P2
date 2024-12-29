using Microsoft.EntityFrameworkCore;
using SWII6P2.Models;

namespace SWII6P2.Services
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Product>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.RecorderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.LastUpdaterId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
        }
    }
}
