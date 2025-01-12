using AshamedApp.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AshamedApp.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<MqttMessageDto> MqttMessages => Set<MqttMessageDto>();
    public DbSet<SnapshotDto> Snapshots => Set<SnapshotDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ensure that DeserializedPayload is not mapped by EF Core
        modelBuilder.Entity<MqttMessageDto>()
            .Ignore(m => m.DeserializedPayload);
        
        modelBuilder.Entity<SnapshotDto>()
            .HasMany(s => s.Messages)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}