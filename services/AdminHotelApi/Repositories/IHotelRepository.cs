using AdminHotelApi.Models;

namespace AdminHotelApi.Repositories;

public interface IHotelRepository
{
    Hotel? GetHotelById(string id);
    Hotel? CreateHotel(CreateHotelDto entity);
    List<Hotel>? GetHotels(string? name = null);
    Hotel? UpdateHotel(string id, UpdateHotelDto entity);
    Hotel? DeleteHotel(string id);
}
