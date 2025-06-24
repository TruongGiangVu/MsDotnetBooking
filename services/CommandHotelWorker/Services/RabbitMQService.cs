using System.Text;
using System.Text.Json;

using CommandHotelWorker.Dtos;
using CommandHotelWorker.Models;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandHotelWorker.Services;

public class RabbitMQService : IRabbitMQService
{
    public QueueDto<Hotel>? ExtractHotelFromEvent(BasicDeliverEventArgs ea)
    {
        byte[]? body = ea.Body.ToArray();
        string json = Encoding.UTF8.GetString(body);
        QueueDto<Hotel>? msg = JsonSerializer.Deserialize<QueueDto<Hotel>>(json);
        return msg;
    }

    public async Task<IConnection> CreateConnection(CancellationToken cancellationToken)
    {
        ConnectionFactory factory = this.CreateFactory();
        return await factory.CreateConnectionAsync(cancellationToken);
    }

    public async Task<IChannel> CreateChannelAndDeclareQueue(IConnection connection, CancellationToken cancellationToken)
    {
        IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        // declare the queue, this will create the queue if it does not exist, or do nothing if it already exists
        await channel.QueueDeclareAsync(
            queue: EnvConfig.RabbitMQ.HotelQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        return channel;
    }

    private ConnectionFactory CreateFactory()
    {
        ConnectionFactory factory = new()
        {
            HostName = EnvConfig.RabbitMQ.Host,
            Port = EnvConfig.RabbitMQ.Port,
            UserName = EnvConfig.RabbitMQ.UserName,
            Password = EnvConfig.RabbitMQ.Password,
        };
        return factory;
    }
}
