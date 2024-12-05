using AshamedApp.Application.DTOs;
using AshamedApp.Domain.Models;

namespace AshamedApp.Application.Repositories;

public interface IMqttMessageRepository
{
    GetAllMqttMessagesResponse GetAllMqttMessages(string topic);
    Task AddMessageToDbAsync(MqttMessageDto message);
}