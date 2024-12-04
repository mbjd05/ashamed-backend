using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;

namespace AshamedApp.Application.Services;

public interface IMqttMessageManagerService
{
    public GetAllMqttMessagesResponse GetAllMqttMessages(string topic);

    public void AddMessage(MqttMessageDto message);
}