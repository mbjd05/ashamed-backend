namespace AshamedApp.Application.DTOs;

public class GetAllSnapshotsResponse(IEnumerable<SnapshotDto> snapshots)
{
    public IEnumerable<SnapshotDto> Snapshots { get; private set; } = snapshots;
}