using Microsoft.EntityFrameworkCore;
using EventRegistrationApi.Models; 

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Event> Events { get; set; }

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

            // Define BLOB field configuration
            /*entity.Property(e => e.Image).HasColumnType("BLOB");*/
        });
    }
}
