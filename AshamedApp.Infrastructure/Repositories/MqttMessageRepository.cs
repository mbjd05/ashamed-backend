using System.Text.RegularExpressions;
using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;
using AshamedApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AshamedApp.Infrastructure.Repositories;

public class MqttMessageRepository(ApplicationDbContext dbContext) : IMqttMessageRepository
{
    public GetAllMqttMessagesResponse GetAllMqttMessages(string topic)
    {
        var mqttMessages = dbContext.MqttMessages.Where(x => x.Topic == topic).ToList();
        return new GetAllMqttMessagesResponse(mqttMessages);
    }
    
    public MqttMessageDto GetLastMqttMessage(string topic)
    {
        return dbContext.MqttMessages
            .Where(x => x.Topic == topic)
            .OrderByDescending(x => x.Timestamp)
            .FirstOrDefault()
            ?? new MqttMessageDto { Topic = topic };
    }


    public async Task AddMessageToDbAsync(MqttMessageDto message)
    {
        dbContext.MqttMessages.Add(new MqttMessageDto
        {
            Id = message.Id,
            Topic = message.Topic,
            Payload = SanitizePayload(message.Payload ?? throw new InvalidOperationException()),
            Timestamp = message.Timestamp
        });
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<MqttMessageDto>> GetMessagesFromDbByTimeRange(string topic, DateTime start, DateTime end)
    {
        var mqttMessages = await dbContext.MqttMessages
            .Where(x => x.Timestamp >= start && x.Timestamp <= end && x.Topic == topic)
            .ToListAsync();
        return mqttMessages;
    }

    public async Task<MqttMessageDto> GetMqttMessageByIdAsync(int id)
    {
        var result = await dbContext.MqttMessages.FindAsync(id);
        return result ?? null;
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