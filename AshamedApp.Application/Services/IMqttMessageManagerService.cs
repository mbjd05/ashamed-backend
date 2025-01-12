using AshamedApp.Application.DTOs;

namespace AshamedApp.Application.Services;

public interface IMqttMessageManagerService
{
    public MqttMessageDto GetLastMqttMessage(string topic);
    public GetAllMqttMessagesResponse GetAllMqttMessages(string topic);

    public Task AddMessageAsync(MqttMessageDto message);

    public Task<List<MqttMessageDto>> GetMessagesFromDbByTimeRangeAsync(string topic, DateTime start, DateTime end);
    
    public Task<MqttMessageDto> GetMqttMessageByIdAsync(int id);
}