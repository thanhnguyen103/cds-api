using CDS_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CDS_API.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Course> Courses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Course entity
            modelBuilder.Entity<Course>()
                .ToTable("Courses")
                .HasKey(c => c.Id);

            modelBuilder.Entity<Course>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Course>()
                .Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Course>()
                .Property(c => c.EffectiveDate)
                .IsRequired();

            modelBuilder.Entity<Course>()
                .Property(c => c.ExpiryDate)
                .IsRequired();
        }
    }
}