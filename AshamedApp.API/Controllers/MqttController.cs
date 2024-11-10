using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AshamedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MqttController(IMqttMessageRepository mqttMessageRepository) : ControllerBase
{
    [HttpGet("{topic}")]
    public ActionResult<GetAllMqttMessagesResponse> GetAllMqttMessages(string topic)
    {
        var response = mqttMessageRepository.GetAllMqttMessages(topic);

        if (response.MqttMessages.Any())
        {
            return Ok(response);
        }

        return NotFound(new { Message = "No messages found for this topic" });
    }
}