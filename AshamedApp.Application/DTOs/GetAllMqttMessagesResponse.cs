namespace AshamedApp.Application.DTOs;

public class GetAllMqttMessagesResponse(IEnumerable<MqttMessageDTO> mqttMessages)
{
    public IEnumerable<MqttMessageDTO> MqttMessages { get; private set; } = mqttMessages;
}