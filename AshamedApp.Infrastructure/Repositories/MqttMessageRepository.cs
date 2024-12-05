using System.Text.RegularExpressions;
using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;
using AshamedApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AshamedApp.Infrastructure.Repositories;

public class MqttMessageRepository(ApplicationDbContext dbContext) : IMqttMessageRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public GetAllMqttMessagesResponse GetAllMqttMessages(string topic)
    {
        List<MqttMessageDto> mqttMessages = _dbContext.MqttMessages.Where(x => x.Topic == topic).ToList();
        return new GetAllMqttMessagesResponse(mqttMessages);
    }

    public async Task AddMessageToDbAsync(MqttMessageDto message)
{
    _dbContext.MqttMessages.Add(new MqttMessageDto
    {
        Topic = message.Topic,
        Payload = SanitizePayload(message.Payload ?? throw new InvalidOperationException()),
        Timestamp = message.Timestamp
    });
    await _dbContext.SaveChangesAsync();
}

    public async Task<List<MqttMessageDto>> GetMessagesFromDbByTimeRange(string topic, DateTime start, DateTime end)
    {
        List<MqttMessageDto> mqttMessages = await _dbContext.MqttMessages
            .Where(x => x.Timestamp >= start && x.Timestamp <= end && x.Topic == topic)
            .ToListAsync();
        return mqttMessages;
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