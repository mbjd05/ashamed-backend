using AshamedApp.Application.DTOs;

namespace AshamedApp.Application.Services;

public interface IMqttMessageManagerService
{
    public GetAllMqttMessagesResponse GetAllMqttMessages(string topic);

    public Task AddMessageAsync(MqttMessageDto message);

    public Task<List<MqttMessageDto>> GetMessagesFromDbByTimeRangeAsync(string topic, DateTime start, DateTime end);
}