using AshamedApp.Application.DTOs;
using AshamedApp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web; // Required for UrlDecode

namespace AshamedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MqttController : ControllerBase
{
    private readonly IMqttMessageManagerService _mqttMessageManagerService;

    public MqttController(IMqttMessageManagerService mqttMessageManagerService)
    {
        _mqttMessageManagerService = mqttMessageManagerService ?? throw new ArgumentNullException(nameof(mqttMessageManagerService));
    }

    // Endpoint to get all messages by topic
    [HttpGet("messages/all/{topic}")]
    public ActionResult<GetAllMqttMessagesResponse> GetAllMqttMessages(string topic)
    {
        // Decode the URL-encoded topic
        topic = HttpUtility.UrlDecode(topic);

        var response = _mqttMessageManagerService.GetAllMqttMessages(topic);

        if (!response.MqttMessages.Any())
        {
            return NotFound(new { Message = "No messages found for this topic" });
        }
        return Ok(response);
    }

    // Endpoint to get messages by time range
    [HttpGet("messages")]
    public async Task<IActionResult> GetMessagesByTimeRange(
        [FromQuery] string topic, 
        [FromQuery] DateTime start, 
        [FromQuery] DateTime end)
    {
        // Decode the URL-encoded topic
        if (!string.IsNullOrWhiteSpace(topic))
        {
            topic = HttpUtility.UrlDecode(topic);
        }
        
        if (string.IsNullOrWhiteSpace(topic))
        {
            return BadRequest(new { Message = "Topic parameter is required." });
        }
        
        var topicExists = _mqttMessageManagerService.GetAllMqttMessages(topic).MqttMessages.Any();
        if (!topicExists)
        {
            return NotFound(new { Message = "No messages exist for the specified topic." });
        }
        
        var messages = await _mqttMessageManagerService.GetMessagesFromDbByTimeRangeAsync(topic, start, end);

        if (!messages.Any())
        {
            return NotFound(new { Message = "No messages found for this topic within the specified time range." });
        }

        return Ok(messages);
    }
}