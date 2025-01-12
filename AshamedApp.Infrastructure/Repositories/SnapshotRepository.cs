using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;
using AshamedApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AshamedApp.Infrastructure.Repositories;

public class SnapshotRepository(ApplicationDbContext dbContext) : ISnapshotRepository
{
    public async Task<SnapshotDto> CreateSnapshotAsync(SnapshotDto snapshot)
    {
        var snapshotToCreate = new SnapshotDto
        {
            Title = snapshot.Title,
            Description = snapshot.Description,
            MessageIds = snapshot.MessageIds
        };

        dbContext.Snapshots.Add(snapshotToCreate);
        await dbContext.SaveChangesAsync();

        return snapshotToCreate;
    }

    public async Task<SnapshotDto?> GetSnapshotByIdAsync(int id)
    {
        var snapshot = await dbContext.Snapshots.FirstOrDefaultAsync(s => s.Id == id);

        if (snapshot == null)
            return null;

        return new SnapshotDto
        {
            Id = snapshot.Id,
            Title = snapshot.Title,
            Description = snapshot.Description,
            MessageIds = snapshot.MessageIds
        };
    }

    public async Task<GetAllSnapshotsResponse> GetAllSnapshotsAsync()
    {
        var snapshots = await dbContext.Snapshots.ToListAsync();

        var snapshotDtos = snapshots.Select(s => new SnapshotDto
        {
            Id = s.Id,
            Title = s.Title,
            Description = s.Description,
            MessageIds = s.MessageIds
        }).ToList();

        return new GetAllSnapshotsResponse(snapshotDtos);
    }

    public async Task<SnapshotDto?> UpdateSnapshotAsync(SnapshotDto snapshot)
    {
        var existingSnapshot = await dbContext.Snapshots.FirstOrDefaultAsync(s => s.Id == snapshot.Id);

        if (existingSnapshot == null)
            return null;

        existingSnapshot.Title = snapshot.Title;
        existingSnapshot.Description = snapshot.Description;
        existingSnapshot.MessageIds = snapshot.MessageIds;

        await dbContext.SaveChangesAsync();

        return new SnapshotDto
        {
            Id = existingSnapshot.Id,
            Title = existingSnapshot.Title,
            Description = existingSnapshot.Description,
            MessageIds = existingSnapshot.MessageIds
        };
    }

    public async Task DeleteSnapshotAsync(int id)
    {
        var snapshot = await dbContext.Snapshots.FindAsync(id);

        if (snapshot != null)
        {
            dbContext.Snapshots.Remove(snapshot);
            await dbContext.SaveChangesAsync();
        }
    }
}