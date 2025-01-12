using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;

namespace AshamedApp.Application.Services.Implementations;

public class SnapshotManagerService(ISnapshotRepository snapshotRepository) : ISnapshotManagerService
{
    public async Task<SnapshotDto> CreateSnapshotAsync(SnapshotDto snapshot)
    {
        var snapshotToCreate = new SnapshotDto
        {
            Title = snapshot.Title,
            Description = snapshot.Description,
            MessageIds = snapshot.MessageIds
        };
    
        var created = await snapshotRepository.CreateSnapshotAsync(snapshotToCreate);
    
        return new SnapshotDto
        {
            Id = created.Id,
            Title = created.Title,
            Description = created.Description,
            MessageIds = created.MessageIds
        };
    }


    public async Task<SnapshotDto?> GetSnapshotByIdAsync(int id)
    {
        return await snapshotRepository.GetSnapshotByIdAsync(id);
    }

    public async Task<GetAllSnapshotsResponse> GetAllSnapshotsAsync()
    {
        return await snapshotRepository.GetAllSnapshotsAsync();
    }

    public async Task<SnapshotDto?> UpdateSnapshotAsync(int id, string title, string description)
    {
        var snapshot = await snapshotRepository.GetSnapshotByIdAsync(id);
        if (snapshot == null) return null;

        snapshot.Title = title;
        snapshot.Description = description;
        var updated = await snapshotRepository.UpdateSnapshotAsync(snapshot);

        return new SnapshotDto()
        {
            Id = updated.Id,
            Title = updated.Title,
            Description = updated.Description
        };
    }

    public async Task DeleteSnapshotAsync(int id)=>
        await snapshotRepository.DeleteSnapshotAsync(id);
}