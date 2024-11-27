namespace AshamedApp.Domain.Models;

public class MqttMessage
{
    public string Topic { get; set; }
    public string Payload { get; set; }
    public DateTime Timestamp { get; set; }
}