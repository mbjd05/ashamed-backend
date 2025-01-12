using AshamedApp.Application.DTOs;

namespace AshamedApp.Application.Repositories;

public interface ISnapshotRepository
{
    Task<SnapshotDto?> CreateSnapshotAsync(SnapshotDto snapshot);
    Task<SnapshotDto?> GetSnapshotByIdAsync(int id);
    Task<GetAllSnapshotsResponse> GetAllSnapshotsAsync();
    Task<SnapshotDto?> UpdateSnapshotAsync(SnapshotDto snapshot);
    Task DeleteSnapshotAsync(int id);
}