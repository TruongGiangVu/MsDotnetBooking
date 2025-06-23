using CommandHotelWorker.Constants;
using CommandHotelWorker.Helper;
using CommandHotelWorker.Models;

using OpenSearch.Client;

namespace CommandHotelWorker.Services;

public class OpenSearchService : IOpenSearchService
{
    private readonly ILogger<OpenSearchService> _logger;
    private readonly IOpenSearchClient _openSearchClient;

    public OpenSearchService(ILogger<OpenSearchService> logger, IOpenSearchClient openSearchClient)
    {
        _logger = logger;
        _openSearchClient = openSearchClient;
    }

    public async Task DoCommandHotelAsync(QueueAction action, Hotel hotel)
    {
        _logger.LogInformation("DoCommandHotel: {action} for hotel: {hotel}", action, hotel.ToJsonString());

        switch (action)
        {
            case QueueAction.Create:
            case QueueAction.Update:
                {
                    IndexResponse response = _openSearchClient.IndexDocument(hotel);
                    _logger.LogInformation("OpenSearch {action} isValid={isValid} id={id}", action.ToString(), response.IsValid, response.Id);
                    break;
                }
            case QueueAction.Delete:
                {
                    DeleteResponse response = await _openSearchClient.DeleteAsync<Hotel>(hotel.Id);
                    _logger.LogInformation("OpenSearch {action} isValid={isValid} id={id}", action.ToString(), response.IsValid, response.Id);
                    break;
                }
            default:
                _logger.LogWarning("Unknown action: {action}", action);
                break;
        }
    }
}
