using AshamedApp.Application.DTOs;

namespace AshamedApp.Application.Repositories;

public interface IMqttMessageRepository
{
    MqttMessageDto GetLastMqttMessage(string topic);
    GetAllMqttMessagesResponse GetAllMqttMessages(string topic);
    Task AddMessageToDbAsync(MqttMessageDto message);
    Task<List<MqttMessageDto>> GetMessagesFromDbByTimeRange(string topic, DateTime start, DateTime end);
    Task<MqttMessageDto> GetMqttMessageByIdAsync(int id);
}