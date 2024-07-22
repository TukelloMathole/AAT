using Microsoft.EntityFrameworkCore;
using EventRegistrationApi.Models; 

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Event> Events { get; set; }
    public DbSet<BookingModel> Bookings { get; set; }
    public DbSet<CategoryModel> Categories { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the Event entity if needed
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId);

            // Optional: Configure properties, constraints, etc.
            entity.Property(e => e.EventName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);

            modelBuilder.Entity<CategoryModel>(entity =>
            {
                entity.HasKey(c => c.CategoryId);
                entity.Property(c => c.CategoryName).IsRequired().HasMaxLength(100);
            });
        });

        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.HasKey(a => a.Email);
            entity.Property(a => a.Password).IsRequired();
        });
    }
}
