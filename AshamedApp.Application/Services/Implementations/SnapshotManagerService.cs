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
                Messages = snapshot.Messages
            };

            var created = await snapshotRepository.CreateSnapshotAsync(snapshotToCreate);

            if (created == null)
            {
                throw new KeyNotFoundException("One or more MqttMessages where not found.");
            }

            return new SnapshotDto
            {
                Id = created.Id,
                Title = created.Title,
                Description = created.Description,
                Messages = created.Messages
            };
    }

    public async Task<SnapshotDto?> GetSnapshotByIdAsync(int id)
    {
        var snapshots = await snapshotRepository.GetSnapshotByIdAsync(id);
        if (snapshots == null)
        {
            throw new KeyNotFoundException($"Snapshot with ID {id} not found.");
        }
        return snapshots;
    }

    public async Task<GetAllSnapshotsResponse> GetAllSnapshotsAsync()
    {
        try
        {
            return await snapshotRepository.GetAllSnapshotsAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred while retrieving all snapshots: {ex.Message}");
        }
    }

    public async Task<SnapshotDto?> UpdateSnapshotAsync(int id, string title, string description)
    {
        try
        {
            var snapshot = await snapshotRepository.GetSnapshotByIdAsync(id);
            if (snapshot == null)
            {
                throw new KeyNotFoundException($"Snapshot with ID {id} not found.");
            }

            snapshot.Title = title;
            snapshot.Description = description;
            var updated = await snapshotRepository.UpdateSnapshotAsync(snapshot);

            return new SnapshotDto
            {
                Id = updated.Id,
                Title = updated.Title,
                Description = updated.Description
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred while updating snapshot: {ex.Message}");
        }
    }

    public async Task DeleteSnapshotAsync(int id)
    {
        try
        {
            await snapshotRepository.DeleteSnapshotAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred while deleting snapshot: {ex.Message}");
        }
    }
}