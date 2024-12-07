using AshamedApp.Application.DTOs;

namespace AshamedApp.Application.Repositories;

public interface IMqttMessageRepository
{
    GetAllMqttMessagesResponse GetAllMqttMessages(string topic);
    Task AddMessageToDbAsync(MqttMessageDto message);
    Task<List<MqttMessageDto>> GetMessagesFromDbByTimeRangeAsync(string topic, DateTime start, DateTime end);
}