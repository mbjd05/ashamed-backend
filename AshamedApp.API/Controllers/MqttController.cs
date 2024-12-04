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
}