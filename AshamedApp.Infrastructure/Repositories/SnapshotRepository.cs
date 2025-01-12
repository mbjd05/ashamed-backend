using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;
using AshamedApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AshamedApp.Infrastructure.Repositories;

public class SnapshotRepository(ApplicationDbContext dbContext) : ISnapshotRepository
{
    public async Task<SnapshotDto> CreateSnapshotAsync(SnapshotDto snapshot)
    {
        dbContext.Snapshots.Add(snapshot);
        await dbContext.SaveChangesAsync();
        return snapshot;
    }

    public async Task<SnapshotDto?> GetSnapshotByIdAsync(int id)=>
        await dbContext.Snapshots.Include(s => s.Messages).FirstOrDefaultAsync(s => s.Id == id);


    public async Task<GetAllSnapshotsResponse> GetAllSnapshotsAsync()
    {
        var snapshots = await dbContext.Snapshots.Include(s => s.Messages).ToListAsync();
        return new GetAllSnapshotsResponse(snapshots);
    }

    public async Task<SnapshotDto?> UpdateSnapshotAsync(SnapshotDto snapshot)
    {
        var existingSnapshot = await dbContext.Snapshots.Include(s => s.Messages).FirstOrDefaultAsync(s => s.Id == snapshot.Id);
        if (existingSnapshot == null) return null;
        
        existingSnapshot.Title = snapshot.Title;
        existingSnapshot.Description = snapshot.Description;

        await dbContext.SaveChangesAsync();
        return existingSnapshot;
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