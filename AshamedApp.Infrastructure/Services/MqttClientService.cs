using MQTTnet;
using MQTTnet.Client;
using System.Text;
using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;
using AshamedApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AshamedApp.Infrastructure.Services;

public class MqttClientService : IDisposable
{
    private readonly IMqttClient _mqttClient;
    private readonly IServiceProvider _serviceProvider; // To create a scope for scoped services
    private readonly string _topic = "z2m/air-monitor";
    private readonly string _brokerAddress = "localhost";
    private readonly int _brokerPort = 8883;

    public MqttClientService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        var mqttFactory = new MqttFactory();
        _mqttClient = mqttFactory.CreateMqttClient();
    }

    public async Task ConnectAsync()
    {
        var options = new MqttClientOptionsBuilder()
            .WithWebSocketServer(options => { options.WithUri($"ws://{_brokerAddress}:{_brokerPort}"); })
            .WithClientId($"DotNetClient-{Guid.NewGuid()}")
            .WithCleanSession()
            .Build();

        _mqttClient.ApplicationMessageReceivedAsync += HandleReceivedMessage;

        await _mqttClient.ConnectAsync(options);
        await _mqttClient.SubscribeAsync(_topic);

        Console.WriteLine("Connected to MQTT broker and subscribed to topic: " + _topic);
    }

    private async Task HandleReceivedMessage(MqttApplicationMessageReceivedEventArgs e)
    {
        var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
        Console.WriteLine($"Received message: {payload}");

        // Create a new scope for the IMqttMessageRepository
        using var scope = _serviceProvider.CreateScope();
        var messageManagerService = scope.ServiceProvider.GetRequiredService<IMqttMessageManagerService>();
        MqttMessageDto messageToSave = new()
        {
            Topic = e.ApplicationMessage.Topic,
            Payload = payload,
            Timestamp = DateTime.UtcNow,
        };
        
        MqttMessageDto lastMessage = messageManagerService.GetLastMqttMessage(e.ApplicationMessage.Topic);
        
        if (lastMessage.Timestamp == default || messageToSave.Timestamp - lastMessage.Timestamp >= TimeSpan.FromMinutes(10))
        {
            await messageManagerService.AddMessageAsync(messageToSave);
        }
    }

    public async Task PublishMessageAsync(string message)
    {
        if (!_mqttClient.IsConnected)
        {
            throw new InvalidOperationException("MQTT client is not connected.");
        }

        var payload = Encoding.UTF8.GetBytes(message);
        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(_topic)
            .WithPayload(payload)
            .Build();

        await _mqttClient.PublishAsync(applicationMessage);
    }

    public void Dispose()
    {
        _mqttClient.Dispose();
    }
}