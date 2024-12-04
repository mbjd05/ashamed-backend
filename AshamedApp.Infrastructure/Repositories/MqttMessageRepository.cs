using System.Text.RegularExpressions;
using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;
using AshamedApp.Domain.Models;
using AshamedApp.Infrastructure.Database;

namespace AshamedApp.Infrastructure.Repositories;

public class MqttMessageRepository(ApplicationDbContext dbContext) : IMqttMessageRepository
{
    public GetAllMqttMessagesResponse GetAllMqttMessages(string topic)
    {
        List<MqttMessageDto> mqttMessages = dbContext.MqttMessages.Where(x => x.Topic == topic).ToList();
        return new GetAllMqttMessagesResponse(mqttMessages);
    }

    public async Task AddMessageAsync(MqttMessageDto message)
{
    dbContext.MqttMessages.Add(new MqttMessageDto
    {
        Topic = message.Topic,
        Payload = SanitizePayload(message.Payload),
        Timestamp = message.Timestamp
    });
    await dbContext.SaveChangesAsync();
}

    private string SanitizePayload(string payload)
    {
        if (string.IsNullOrEmpty(payload))
        {
            return payload;
        }
        payload = Regex.Replace(payload, @"\b(Infinity|-Infinity|NaN)\b", "0");

        return payload;
    }
}