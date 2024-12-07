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

        if(!response.MqttMessages.Any())
        {
            return NotFound(new { Message = "No messages found for this topic" });
        }
        return Ok(response);
    }

    [HttpGet("by-time-range")]
    public async Task<ActionResult<List<MqttMessageDto>>> GetMessagesFromDbByTimeRange([FromQuery] TimeRangeRequest request)
    {
        var topicExists = mqttMessageManagerService.GetAllMqttMessages(request.Topic).MqttMessages.Any();

        if (!topicExists)
        {
            return NotFound(new { Message = "No messages exist for the specified topic." });
        }

        var response =
            await mqttMessageManagerService.GetMessagesFromDbByTimeRangeAsync(request.Topic, request.Start, request.End);

        if (!response.Any())
        {
            return NotFound(new { Message = "No messages found for this topic within the specified time range." });
        }
        return Ok(response);
    }
}