using AshamedApp.Application.DTOs;

namespace AshamedApp.Application.Repositories;

public interface IMqttMessageRepository
{
    IEnumerable<MqttMessageDTO> GetAllMqttMessages(string topic);
}