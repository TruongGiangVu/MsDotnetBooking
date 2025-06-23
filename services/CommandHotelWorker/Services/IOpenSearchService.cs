using CommandHotelWorker.Constants;
using CommandHotelWorker.Models;

namespace CommandHotelWorker.Services;

public interface IOpenSearchService
{
    Task DoCommandHotelAsync(QueueAction action, Hotel hotel);
}
