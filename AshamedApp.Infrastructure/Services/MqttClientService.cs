using MQTTnet;
using MQTTnet.Client;
using System.Text;
using AshamedApp.Application.DTOs;
using AshamedApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AshamedApp.Infrastructure.Services;

public class MqttClientService : IDisposable
{
    private readonly IMqttClient _mqttClient;
    private readonly IServiceProvider _serviceProvider; // To create a scope for scoped services
    private readonly string _topic = "z2m/air-monitor";
    private readonly string _brokerAddress; // Broker address dynamically loaded from config
    private readonly int _brokerPort;
    private Dictionary<string, DateTime> _lastMessageTimestamps = new Dictionary<string, DateTime>();
    private readonly ILogger<MqttClientService> _logger;

    public MqttClientService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<MqttClientService> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Load configuration from appsettings
        _brokerAddress = configuration["MqttSettings:BrokerAddress"] ?? "localhost";
        _brokerPort = int.TryParse(configuration["MqttSettings:BrokerPort"], out var port) ? port : 8883;

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

        try
        {
            await _mqttClient.ConnectAsync(options);
            await _mqttClient.SubscribeAsync(_topic);
            _logger.LogInformation($"Connected to MQTT broker at {_brokerAddress}:{_brokerPort} and subscribed to topic: {_topic}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to MQTT broker.");
        }
    }

    private async Task HandleReceivedMessage(MqttApplicationMessageReceivedEventArgs e)
    {
        var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

        using var scope = _serviceProvider.CreateScope();
        var messageManagerService = scope.ServiceProvider.GetRequiredService<IMqttMessageManagerService>();
        var currentTime = DateTime.UtcNow;

        MqttMessageDto messageToSave = new()
        {
            Topic = e.ApplicationMessage.Topic,
            Payload = payload,
            Timestamp = currentTime,
        };

        if (!_lastMessageTimestamps.TryGetValue(e.ApplicationMessage.Topic, out DateTime lastTimestamp))
        {
            var lastMessage = messageManagerService.GetLastMqttMessage(e.ApplicationMessage.Topic);
            lastTimestamp = lastMessage.Timestamp;
            _lastMessageTimestamps[e.ApplicationMessage.Topic] = lastTimestamp;
        }

        var timeSinceLastMessage = currentTime - lastTimestamp;

        if (lastTimestamp == default || timeSinceLastMessage >= TimeSpan.FromMinutes(10))
        {
            await messageManagerService.AddMessageAsync(messageToSave);
            _lastMessageTimestamps[e.ApplicationMessage.Topic] = currentTime;
            _logger.LogInformation($"Saved message from topic {e.ApplicationMessage.Topic} at {currentTime}");
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
        _logger.LogInformation($"Published message to topic {_topic}: {message}");
    }

    public void Dispose()
    {
        _mqttClient.Dispose();
    }
}