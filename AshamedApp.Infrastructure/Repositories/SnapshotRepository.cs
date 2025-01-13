using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;
using AshamedApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AshamedApp.Infrastructure.Repositories;

public class SnapshotRepository(ApplicationDbContext dbContext) : ISnapshotRepository
{
    public async Task<SnapshotDto?> CreateSnapshotAsync(SnapshotDto snapshot)
    {
        // Handle existing MqttMessages or return null if any message does not exist
        var messages = await HandleExistingMqttMessages(snapshot.Messages);
        if (messages == null)
        {
            return null;
        }

        // Create the SnapshotDto
        var snapshotToCreate = new SnapshotDto
        {
            Title = snapshot.Title,
            Description = snapshot.Description,
            Messages = messages
        };

        // Add the snapshot to the database
        dbContext.Snapshots.Add(snapshotToCreate);
        await dbContext.SaveChangesAsync();
    
        return snapshotToCreate;
    }

    private async Task<List<MqttMessageDto>?> HandleExistingMqttMessages(List<MqttMessageDto> messages)
    {
        var processedMessages = new List<MqttMessageDto>();

        foreach (var message in messages)
        {
            var existingMessage = await dbContext.MqttMessages
                .FirstOrDefaultAsync(m => m.Id == message.Id);

            if (existingMessage != null)
            {
                processedMessages.Add(existingMessage);
            }
            else
            {
                return null;
            }
        }

        return processedMessages;
    }

    public async Task<GetAllSnapshotsResponse> GetAllSnapshotsAsync()
    {
        var snapshots = await dbContext.Snapshots
            .Include(s => s.Messages) 
            .ToListAsync();

        var snapshotDtos = snapshots.Select(s => new SnapshotDto
        {
            Id = s.Id,
            Title = s.Title,
            Description = s.Description,
            Messages = s.Messages
        }).ToList();

        return new GetAllSnapshotsResponse(snapshotDtos);
    }

    public async Task<SnapshotDto?> UpdateSnapshotAsync(SnapshotDto snapshot)
    {
        var existingSnapshot = await dbContext.Snapshots
            .Include(s => s.Messages) // Load related messages
            .FirstOrDefaultAsync(s => s.Id == snapshot.Id);

        if (existingSnapshot == null)
            return null;

        // Update the snapshot's details
        existingSnapshot.Title = snapshot.Title;
        existingSnapshot.Description = snapshot.Description;

        // Save the changes
        await dbContext.SaveChangesAsync();

        return existingSnapshot;
    }

    public async Task DeleteSnapshotAsync(int id)
    {
        var snapshot = await dbContext.Snapshots
            .Include(s => s.Messages) // Load related messages
            .FirstOrDefaultAsync(s => s.Id == id);

        if (snapshot != null)
        {
            dbContext.Snapshots.Remove(snapshot);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<SnapshotDto?> GetSnapshotByIdAsync(int id)
    {
        
        var snapshots = await dbContext.Snapshots
            .Include(s => s.Messages) // Ensure related messages are loaded
            .FirstOrDefaultAsync(s => s.Id == id);
        return snapshots ?? null;
    }
}
