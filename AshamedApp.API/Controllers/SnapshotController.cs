using AshamedApp.Application.DTOs;
using AshamedApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AshamedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SnapshotController(ISnapshotManagerService snapshotManagerService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSnapshotAsync(SnapshotDto snapshot)
    {
        var created = await snapshotManagerService.CreateSnapshotAsync(snapshot);
        return Ok(created);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSnapshotByIdAsync(int id)
    {
        var snapshot = await snapshotManagerService.GetSnapshotByIdAsync(id);
        if (snapshot == null) return NotFound();
        return Ok(snapshot);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllSnapshotsAsync()
    {
        var allSnapshots = await snapshotManagerService.GetAllSnapshotsAsync();
        if (!allSnapshots.Snapshots.Any()) return NotFound();
        return Ok(allSnapshots);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateSnapshot(int id, SnapshotDto snapshotDto)
    {
        if (snapshotDto == null)
        {
            return BadRequest(new { Message = "Request body cannot be null." });
        }
        
        var existingSnapshot = await snapshotManagerService.GetSnapshotByIdAsync(id);
        if (existingSnapshot == null)
        {
            return NotFound(new { Message = $"Snapshot with ID {id} not found." });
        }
        
        existingSnapshot.Title = snapshotDto.Title;
        existingSnapshot.Description = snapshotDto.Description;
        var updatedSnapshot = await snapshotManagerService.UpdateSnapshotAsync(id, snapshotDto.Title, snapshotDto.Description);

        return Ok(updatedSnapshot);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSnapshotAsync(int id)
    {
        await snapshotManagerService.DeleteSnapshotAsync(id);
        return NoContent();
    }
}