namespace AshamedApp.Application.DTOs;

public class GetAllMqttMessagesResponse(IEnumerable<MqttMessageDto> mqttMessages)
{
    public IEnumerable<MqttMessageDto> MqttMessages { get; private set; } = mqttMessages;
}