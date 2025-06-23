using System.Text;
using System.Text.Json;

using AdminHotelApi.Models;

using RabbitMQ.Client;

namespace AdminHotelApi.Services;

public class RabbitMQService : IRabbitMQService
{
    private readonly ILogger<RabbitMQService> _logger;

    public RabbitMQService(ILogger<RabbitMQService> logger)
    {
        _logger = logger;
    }
    public async Task PushAsync(QueueAction action, Hotel hotel)
    {
        ConnectionFactory factory = new()
        {
            HostName = EnvConfig.RabbitMQ.Host,
            Port = EnvConfig.RabbitMQ.Port,
            UserName = EnvConfig.RabbitMQ.UserName,
            Password = EnvConfig.RabbitMQ.Password
        };

        using IConnection connection = await factory.CreateConnectionAsync();
        using IChannel channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: EnvConfig.RabbitMQ.HotelQueue,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        string message = JsonSerializer.Serialize(new QueueDto<Hotel>
        {
            Action = action,
            Payload = hotel
        });

        _logger.LogInformation("Publishing queue with action: {Action} and payload: {Payload}", action, hotel.ToJsonString());
        byte[]? body = Encoding.UTF8.GetBytes(message);

        BasicProperties props = new();
        await channel.BasicPublishAsync(exchange: "",
                                 routingKey: EnvConfig.RabbitMQ.HotelQueue,
                                 mandatory: true,
                                 basicProperties: props,
                                 body: body);
    }
}
