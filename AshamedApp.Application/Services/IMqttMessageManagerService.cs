using AshamedApp.Application.DTOs;

namespace AshamedApp.Application.Services;

public interface IMqttMessageManagerService
{
    public GetAllMqttMessagesResponse GetAllMqttMessages(string topic);

    public void AddMessage(MqttMessageDto message);

    public Task<List<MqttMessageDto>> GetMessagesFromDbByTimeRange(string topic, DateTime start, DateTime end);
}