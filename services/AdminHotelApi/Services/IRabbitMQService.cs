using AdminHotelApi.Models;

namespace AdminHotelApi.Services;

public interface IRabbitMQService
{
    Task PushAsync(QueueAction action, Hotel hotel);
}
