using EventRegistrationApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> _logger; 

    public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger) : base(options)
    {
        _logger = logger;
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<BookingModel> Bookings { get; set; }
    public DbSet<CategoryModel> Categories { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _logger.LogInformation("Configuring model...");

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId);
            entity.Property(e => e.EventName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);

            _logger.LogInformation("Configured Event entity.");
        });

        modelBuilder.Entity<CategoryModel>(entity =>
        {
            entity.HasKey(c => c.CategoryId);
            entity.Property(c => c.CategoryName).IsRequired().HasMaxLength(100);

            _logger.LogInformation("Configured CategoryModel entity.");
        });

        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.HasKey(a => a.Email);
            entity.Property(a => a.Password).IsRequired();

            _logger.LogInformation("Configured AdminUser entity.");
        });
    }
}
