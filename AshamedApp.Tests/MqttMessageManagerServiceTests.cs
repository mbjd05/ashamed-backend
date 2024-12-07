using AshamedApp.Application.Services.Implementations;
using AshamedApp.Application.Repositories;
using AshamedApp.Application.DTOs;
using Moq;
using Xunit;

namespace AshamedApp.Tests;

public class MqttMessageManagerServiceTests
{
    private readonly Mock<IMqttMessageRepository> _mockRepository;
    private readonly MqttMessageManagerService _service;

    public MqttMessageManagerServiceTests()
    {
        // Initialize the mock repository
        _mockRepository = new Mock<IMqttMessageRepository>();

        // Create an instance of MqttMessageManagerService with the mocked repository
        _service = new MqttMessageManagerService(_mockRepository.Object);
    }

    [Fact]
    public void GetAllMqttMessages_ReturnsMessages_WhenTopicExists()
    {
        // Arrange
        string topic = "test/topic";
        var expectedMqttMessages = new List<MqttMessageDto>
        {
            new MqttMessageDto { Topic = topic, Payload = "Message 1", Timestamp = DateTime.Now },
            new MqttMessageDto { Topic = topic, Payload = "Message 2", Timestamp = DateTime.Now }
        };
        _mockRepository.Setup(repo => repo.GetAllMqttMessages(topic))
            .Returns(new GetAllMqttMessagesResponse(expectedMqttMessages));

        // Act
        var actualMqttMessagesResponse = _service.GetAllMqttMessages(topic);

        // Assert
        Assert.NotNull(actualMqttMessagesResponse);
        Assert.Equal(expectedMqttMessages.Count, actualMqttMessagesResponse.MqttMessages.Count());

        foreach (var expectedMessage in expectedMqttMessages)
        {
            Assert.Contains(actualMqttMessagesResponse.MqttMessages, actualMessage =>
                actualMessage.Topic == expectedMessage.Topic &&
                actualMessage.Payload == expectedMessage.Payload &&
                actualMessage.Timestamp == expectedMessage.Timestamp);
        }

        // Verify repository interaction
        _mockRepository.Verify(repo => repo.GetAllMqttMessages(topic), Times.Once);
    }

    [Fact]
    public void GetAllMqttMessages_ReturnsEmpty_WhenNoMessagesExist()
    {
        // Arrange
        string topic = "nonexistent/topic";
        _mockRepository.Setup(repo => repo.GetAllMqttMessages(topic))
            .Returns(new GetAllMqttMessagesResponse(Enumerable.Empty<MqttMessageDto>()));

        // Act
        var result = _service.GetAllMqttMessages(topic);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.MqttMessages);
    }

    [Fact]
public async Task GetMessagesFromDbByTimeRange_ReturnsMessages_WhenMessagesExistInTimeRange()
{
    // Arrange
    string topic = "test/topic";
    DateTime start = DateTime.Now.AddHours(-1);
    DateTime end = DateTime.Now;
    var expectedMqttMessages = new List<MqttMessageDto>
    {
        new MqttMessageDto { Topic = topic, Payload = "Message 1", Timestamp = start.AddMinutes(10) },
        new MqttMessageDto { Topic = topic, Payload = "Message 2", Timestamp = start.AddMinutes(20) }
    };

    _mockRepository
        .Setup(repo => repo.GetMessagesFromDbByTimeRange(topic, start, end))
        .ReturnsAsync(expectedMqttMessages);

    // Act
    var actualMqttMessages = await _service.GetMessagesFromDbByTimeRange(topic, start, end);

    // Assert
    Assert.NotNull(actualMqttMessages);
    Assert.Equal(expectedMqttMessages.Count, actualMqttMessages.Count);
    foreach (var expectedMessage in expectedMqttMessages)
    {
        Assert.Contains(actualMqttMessages, actualMessage =>
            actualMessage.Topic == expectedMessage.Topic &&
            actualMessage.Payload == expectedMessage.Payload &&
            actualMessage.Timestamp == expectedMessage.Timestamp);
    }

    // Verify repository interaction
    _mockRepository.Verify(repo => repo.GetMessagesFromDbByTimeRange(topic, start, end), Times.Once);
}

[Fact]
public async Task GetMessagesFromDbByTimeRange_ReturnsEmptyList_WhenNoMessagesExistInTimeRange()
{
    // Arrange
    string topic = "test/topic";
    DateTime start = DateTime.Now.AddHours(-1);
    DateTime end = DateTime.Now;

    _mockRepository
        .Setup(repo => repo.GetMessagesFromDbByTimeRange(topic, start, end))
        .ReturnsAsync(new List<MqttMessageDto>());

    // Act
    var actualMqttMessages = await _service.GetMessagesFromDbByTimeRange(topic, start, end);

    // Assert
    Assert.NotNull(actualMqttMessages);
    Assert.Empty(actualMqttMessages);

    // Verify repository interaction
    _mockRepository.Verify(repo => repo.GetMessagesFromDbByTimeRange(topic, start, end), Times.Once);
}

[Fact]
public async Task GetMessagesFromDbByTimeRange_ThrowsException_WhenRepositoryThrowsException()
{
    // Arrange
    string topic = "test/topic";
    DateTime start = DateTime.Now.AddHours(-1);
    DateTime end = DateTime.Now;

    _mockRepository
        .Setup(repo => repo.GetMessagesFromDbByTimeRange(topic, start, end))
        .ThrowsAsync(new Exception("Database error"));

    // Act & Assert
    var exception = await Assert.ThrowsAsync<Exception>(async () =>
        await _service.GetMessagesFromDbByTimeRange(topic, start, end));
    Assert.Equal("Database error", exception.Message);

    // Verify repository interaction
    _mockRepository.Verify(repo => repo.GetMessagesFromDbByTimeRange(topic, start, end), Times.Once);
}
}