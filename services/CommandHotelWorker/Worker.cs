using System.Text;
using System.Text.Json;

using CommandHotelWorker.Dtos;
using CommandHotelWorker.Helper;
using CommandHotelWorker.Models;
using CommandHotelWorker.Services;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace CommandHotelWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IOpenSearchService _openSearchService;
    private readonly IRabbitMQService _rabbitMQService;
    private IConnection? _connection;
    private IChannel? _channel;

    public Worker(ILogger<Worker> logger,
                IOpenSearchService openSearchService,
                IRabbitMQService rabbitMQService)
    {
        _logger = logger;
        _openSearchService = openSearchService;
        _rabbitMQService = rabbitMQService;
    }
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _connection = await _rabbitMQService.CreateConnection(cancellationToken);
        _channel = await _rabbitMQService.CreateChannelAndDeclareQueue(_connection, cancellationToken);
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_channel is null)
            {
                _logger.LogError("Channel is null, cannot consume messages.");
                return;
            }

            AsyncEventingBasicConsumer consumer = new(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    QueueDto<Hotel>? msg = _rabbitMQService.ExtractHotelFromEvent(ea);
                    _logger.LogInformation("[✔] Hotel Received (time): {time}, {msg}", DateTime.Now, msg.ToJsonString());
                    Hotel? hotel = msg?.Payload;
                    if (msg is null || hotel is null)
                    {
                        _logger.LogError("Hotel is null, cannot process.");
                    }
                    else
                    {
                        await _openSearchService.DoCommandHotelAsync(msg.Action, hotel);
                    }
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                }
            };
            await _channel.BasicConsumeAsync(queue: EnvConfig.RabbitMQ.HotelQueue, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        }
    }

    public override void Dispose()
    {
        _logger.LogInformation("Worker Dispose...");
        _channel?.Dispose();
        _connection?.Dispose();
        GC.SuppressFinalize(this); // ✅ Suppress finalize (even if one doesn't exist)
        base.Dispose();
    }
}
