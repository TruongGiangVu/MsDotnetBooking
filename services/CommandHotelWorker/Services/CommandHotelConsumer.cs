using Core.Dtos;
using Core.Helper;
using Core.Models;

using MassTransit;

using OpenSearch.Client;

using Serilog;

namespace CommandHotelWorker.Services;

public class CommandHotelConsumer : IConsumer<QueueDto<Hotel>>
{
    private readonly IOpenSearchClient _openSearchClient;

    public CommandHotelConsumer(IOpenSearchClient openSearchClient)
    {
        _openSearchClient = openSearchClient;
    }
    public Task Consume(ConsumeContext<QueueDto<Hotel>> context)
    {
        QueueDto<Hotel> message = context.Message;
        Hotel? hotel = message.Payload;
        Log.Information($"[✔] Hotel Received ({DateTime.Now}): {message.ToJsonString()}");

        if (hotel is null)
        {
            Log.Error("Hotel is null, cannot process.");
            return Task.CompletedTask;
        }

        IndexResponse response = _openSearchClient.IndexDocument<Hotel>(hotel);
        Log.Information("OpenSearch isValid={isValid} id={id}", response.IsValid, response.Id);

        return Task.CompletedTask;
    }
}
