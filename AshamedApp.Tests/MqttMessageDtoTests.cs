using System.Text.Json;
using AshamedApp.Application.DTOs;
using Xunit;

namespace AshamedApp.Tests;

public class MqttMessageDtoTests
{
    [Fact]
        public void DeserializedPayload_ReturnsNull_WhenPayloadIsNull()
        {
            // Arrange
            var message = new MqttMessageDto
            {
                Topic = "test/topic",
                Payload = null,
                Timestamp = DateTime.Now
            };

            // Act
            var result = message.DeserializedPayload;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void DeserializedPayload_ReturnsNull_WhenPayloadIsEmpty()
        {
            // Arrange
            var message = new MqttMessageDto
            {
                Topic = "test/topic",
                Payload = "",
                Timestamp = DateTime.Now
            };

            // Act
            var result = message.DeserializedPayload;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void DeserializedPayload_ReturnsDeserializedObject_WhenPayloadIsJson()
        {
            // Arrange
            var message = new MqttMessageDto
            {
                Topic = "test/topic",
                Payload = "{\"key\": \"value\"}",
                Timestamp = DateTime.Now
            };

            // Act
            var result = message.DeserializedPayload;

            // Assert
            Assert.IsType<JsonElement>(result);
            var jsonResult = (JsonElement)result;
            Assert.Equal("value", jsonResult.GetProperty("key").GetString());
        }

        [Fact]
        public void DeserializedPayload_ReturnsPayload_WhenPayloadIsNotJson()
        {
            // Arrange
            var message = new MqttMessageDto
            {
                Topic = "test/topic",
                Payload = "Non-JSON string",
                Timestamp = DateTime.Now
            };

            // Act
            var result = message.DeserializedPayload;

            // Assert
            Assert.Equal("Non-JSON string", result);
        }

        [Fact]
        public void DeserializedPayload_ReturnsPayload_WhenPayloadIsInvalidJson()
        {
            // Arrange
            var message = new MqttMessageDto
            {
                Topic = "test/topic",
                Payload = "{invalidJson}",
                Timestamp = DateTime.Now
            };

            // Act
            var result = message.DeserializedPayload;

            // Assert
            Assert.Equal("{invalidJson}", result);  // Should return the original Payload
        }
}