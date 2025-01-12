using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace AshamedApp.Application.DTOs;

public class MqttMessageDto
{
    public int Id { get; init; }
    public required string Topic { get; init; }
    public string? Payload { get; init; }
    public DateTime Timestamp { get; init; }

    public object? DeserializedPayload
    {
        get
        {
            if (string.IsNullOrEmpty(Payload))
            {
                return null;
            }

            try
            {
                var cleanedPayload = Payload.Trim();
                if (cleanedPayload.StartsWith("{") || cleanedPayload.StartsWith("["))
                {
                    return JsonSerializer.Deserialize<object>(cleanedPayload);
                }

                return Payload;
            }
            catch
            {
                return Payload;
            }
        }
    }
}