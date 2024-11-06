using AshamedApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AshamedApp.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<MqttMessage> MqttMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MqttMessage>().ToTable("MqttMessages");
    }
}