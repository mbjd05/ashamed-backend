using AshamedApp.Application.DTOs;
using AshamedApp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web; // Required for UrlDecode

namespace AshamedApp.API.Controllers;

[ApiController]
[Route("api/mqtt")]
public class MqttController(IMqttMessageManagerService mqttMessageManagerService) : ControllerBase
{
    private readonly IMqttMessageManagerService _mqttMessageManagerService = mqttMessageManagerService ?? throw new ArgumentNullException(nameof(mqttMessageManagerService));

    // Endpoint to get all messages by topic
    [HttpGet("{topic}/messages")]
    public ActionResult<GetAllMqttMessagesResponse> GetAllMqttMessages(string topic)
    {
        topic = HttpUtility.UrlDecode(topic);
        var response = _mqttMessageManagerService.GetAllMqttMessages(topic);
        if (!response.MqttMessages.Any())
        {
            return NotFound(new { Message = "No messages found for this topic" });
        }
        return Ok(response);
    }

    // Endpoint to get messages by time range
    [HttpGet("{topic}/messages-by-time-range")]
    public async Task<IActionResult> GetMessagesByTimeRange(
        string topic, 
        [FromQuery] DateTime start, 
        [FromQuery] DateTime end)
    {
        topic = HttpUtility.UrlDecode(topic);
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
    
    [HttpGet("messages/{id}")]
    public async Task<ActionResult<MqttMessageDto>> GetMqttMessageById(int id)
    {
        var message = await _mqttMessageManagerService.GetMqttMessageByIdAsync(id);
        if (message == null)
        {
            return NotFound(new { Message = "No message found with the provided ID." });
        }
        return Ok(message);
    }
}