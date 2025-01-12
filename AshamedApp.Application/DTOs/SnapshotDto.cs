namespace AshamedApp.Application.DTOs;

public class SnapshotDto
{
    public int Id { get; init; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public List<int> MessageIds { get; set; } = new();
}