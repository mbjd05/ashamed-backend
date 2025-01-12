using AshamedApp.Application.DTOs;

namespace AshamedApp.Application.Services;

public interface ISnapshotManagerService
{
    Task<SnapshotDto> CreateSnapshotAsync(SnapshotDto snapshot);
    Task<SnapshotDto?> GetSnapshotByIdAsync(int id);
    Task<GetAllSnapshotsResponse> GetAllSnapshotsAsync();
    Task<SnapshotDto?> UpdateSnapshotAsync(int id, string title, string description);
    Task DeleteSnapshotAsync(int id);
}