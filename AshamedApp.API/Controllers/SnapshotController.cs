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
        try
        {
            var created = await snapshotManagerService.CreateSnapshotAsync(snapshot);
            return Ok(created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = "MqttMessage not found.", details = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred while creating the snapshot.", details = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSnapshotByIdAsync(int id)
    {
        try
        {
            var snapshot = await snapshotManagerService.GetSnapshotByIdAsync(id);
            if (snapshot == null) return NotFound();
            return Ok(snapshot);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = "Snapshot not found", details = e.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { Message = e.Message });
        }
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
