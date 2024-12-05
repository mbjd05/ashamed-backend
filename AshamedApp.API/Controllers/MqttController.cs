using AshamedApp.Application.DTOs;
using AshamedApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AshamedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MqttController(IMqttMessageManagerService mqttMessageManagerService) : ControllerBase
{
    [HttpGet("{topic}")]
    public ActionResult<GetAllMqttMessagesResponse> GetAllMqttMessages(string topic)
    {
        var response = mqttMessageManagerService.GetAllMqttMessages(topic);

        if (response.MqttMessages.Any())
        {
            return Ok(response);
        }

        return NotFound(new { Message = "No messages found for this topic" });
    }

    [HttpGet("by-time-range")]
    public async Task<ActionResult<List<MqttMessageDto>>> GetMessagesFromDbByTimeRange(string topic,  DateTime start, DateTime end)
    {
        var topicExists = mqttMessageManagerService.GetAllMqttMessages(topic).MqttMessages.Any();

        if (!topicExists)
        {
            return NotFound(new { Message = "No messages exist for the specified topic." });
        }

        var response = await mqttMessageManagerService.GetMessagesFromDbByTimeRange(topic, start, end);

        if (start > end)
        {
            return BadRequest(new { Message = "The start date must be earlier than the end date."});
        }

        if (response.Any())
        {
            return Ok(response);
        }

        return NotFound(new { Message = "No messages found for this topic within the specified time range." });
    }

}