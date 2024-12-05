using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;

namespace AshamedApp.Application.Services.Implementations;

public class MqttMessageManagerService(IMqttMessageRepository mqttMessageRepository) : IMqttMessageManagerService
{
    public GetAllMqttMessagesResponse GetAllMqttMessages(string topic)
    {
        return mqttMessageRepository.GetAllMqttMessages(topic);
    }

    public void AddMessage(MqttMessageDto message)
    {
        mqttMessageRepository.AddMessageToDbAsync(message);
    }

    public async Task<List<MqttMessageDto>> GetMessagesFromDbByTimeRange(string topic, DateTime start, DateTime end)
    {
        return await mqttMessageRepository.GetMessagesFromDbByTimeRange(topic, start, end);
    }
}