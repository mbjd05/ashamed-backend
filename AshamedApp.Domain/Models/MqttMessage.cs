namespace AshamedApp.Domain.Models;

public class MqttMessage
{
    public string Topic { get; set; }
    public string Message { get; set; }
    public DateTime TimeStamp { get; set; }
}