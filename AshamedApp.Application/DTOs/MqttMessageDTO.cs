namespace AshamedApp.Application.DTOs;

public class MqttMessageDTO
{
    public string Topic { get; set; }
    public string Payload { get; set; }
    public DateTime Timestamp { get; set; }
}