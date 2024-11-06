using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;
using AshamedApp.Infrastructure.Database;

namespace AshamedApp.Infrastructure.Repositories;

public class MqttMessageRepository : IMqttMessageRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MqttMessageRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IEnumerable<MqttMessageDTO> GetAllMqttMessages(string topic)
    {
        var mqttMessages = _dbContext.MqttMessages.Where(x => x.Topic == topic).ToList();
        return mqttMessages;
    }

}