using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;

namespace AshamedApp.Application.Services.Implementations;

public class MqttMessageManagerService(IMqttMessageRepository mqttMessageRepository) : IMqttMessageManagerService
{
    public MqttMessageDto GetLastMqttMessage(string topic)
    {
        return mqttMessageRepository.GetLastMqttMessage(topic);
    }

    public GetAllMqttMessagesResponse GetAllMqttMessages(string topic)
    {
        return mqttMessageRepository.GetAllMqttMessages(topic);
    }

    public async Task AddMessageAsync(MqttMessageDto message)
    {
        await mqttMessageRepository.AddMessageToDbAsync(message);
    }

    public async Task<List<MqttMessageDto>> GetMessagesFromDbByTimeRangeAsync(string topic, DateTime start, DateTime end)
    {
        return await mqttMessageRepository.GetMessagesFromDbByTimeRange(topic, start, end);
    }

    public async Task<MqttMessageDto> GetMqttMessageByIdAsync(int id)
    {
        return await mqttMessageRepository.GetMqttMessageByIdAsync(id);
    }
}