namespace AshamedApp.Domain.Models;

public class MqttMessage
{
    public required string Topic { get; set; }
    public required string Payload { get; set; }
    public DateTime Timestamp { get; set; }
}