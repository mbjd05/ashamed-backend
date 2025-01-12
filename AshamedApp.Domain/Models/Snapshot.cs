namespace AshamedApp.Domain.Models;

public class Snapshot
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<MqttMessage> Messages { get; set; } = new List<MqttMessage>();
}