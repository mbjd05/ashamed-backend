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

        modelBuilder.Entity<MqttMessageDto>()
            .Ignore(m => m.DeserializedPayload);

        modelBuilder.Entity<SnapshotDto>()
            .HasMany(s => s.Messages)  // Snapshot has many messages
            .WithMany() 
            .UsingEntity<Dictionary<string, object>>(
                "SnapshotMqttMessage",
                l => l.HasOne<MqttMessageDto>()
                    .WithMany()
                    .HasForeignKey("MqttMessageId")
                    .OnDelete(DeleteBehavior.Restrict),
                r => r.HasOne<SnapshotDto>()
                    .WithMany()
                    .HasForeignKey("SnapshotId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("SnapshotId", "MqttMessageId");
                    j.ToTable("SnapshotMqttMessages");
                });
    }
}