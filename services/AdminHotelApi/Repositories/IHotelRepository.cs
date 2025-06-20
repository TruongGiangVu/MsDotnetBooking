using Core.Models;

namespace AdminHotelApi.Repositories;

public interface IHotelRepository
{
    Hotel? GetHotelById(string id);
    Hotel? CreateHotel(CreateHotelDto entity);
    List<Hotel>? GetHotel(string name);
    // Hotel? UpdateHotel(string id, UpdateHotelDto entity);
    // void DeleteHotel(string id);
    // List<Hotel> GetAllHotels();
    // List<Hotel> GetHotelsByName(string name);
}
