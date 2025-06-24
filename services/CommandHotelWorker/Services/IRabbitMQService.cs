using CommandHotelWorker.Dtos;
using CommandHotelWorker.Models;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandHotelWorker.Services;

public interface IRabbitMQService
{
    QueueDto<Hotel>? ExtractHotelFromEvent(BasicDeliverEventArgs ea);
    Task<IConnection> CreateConnection(CancellationToken cancellationToken);
    Task<IChannel> CreateChannelAndDeclareQueue(IConnection connection, CancellationToken cancellationToken);
}
