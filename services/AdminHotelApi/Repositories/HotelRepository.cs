using AdminHotelApi.Models;

namespace AdminHotelApi.Repositories;
public class HotelRepository : IHotelRepository
{
    private readonly AppDbContext _db;

    public HotelRepository(AppDbContext db)
    {
        _db = db;
    }
    public Hotel? GetHotelById(string id)
    {
        return _db.Hotels.Where(p => p.Id == id).FirstOrDefault();
    }

    public Hotel? CreateHotel(CreateHotelDto entity)
    {
        var newHotel = new Hotel
        {
            Id = Guid.NewGuid().ToString(),
            Name = entity.Name,
            Description = entity.Description,
            HumanAmount = entity.HumanAmount,
            Price = entity.Price,
            RoomCode = entity.RoomCode,
            IsVip = entity.IsVip,
            OpenDate = DateTime.SpecifyKind(entity.OpenDate ?? DateTime.Now, DateTimeKind.Utc),
        };
        _db.Hotels.Add(newHotel);
        _db.SaveChanges();
        return newHotel;
    }

    public Hotel? UpdateHotel(string id, UpdateHotelDto entity)
    {
        Hotel? hotel = _db.Hotels.Find(id);
        if (hotel is null) return null;

        hotel.Name = entity.Name;
        hotel.Description = entity.Description;
        hotel.HumanAmount = entity.HumanAmount;
        hotel.Price = entity.Price;
        hotel.RoomCode = entity.RoomCode;
        hotel.IsVip = entity.IsVip;
        hotel.OpenDate = DateTime.SpecifyKind(entity.OpenDate ?? DateTime.Now, DateTimeKind.Utc);

        _db.Hotels.Update(hotel);
        _db.SaveChanges();
        return hotel;
    }

    public Hotel? DeleteHotel(string id)
    {
        Hotel? hotel = _db.Hotels.Find(id);
        if (hotel is null) return null;

        _db.Hotels.Remove(hotel);
        _db.SaveChanges();
        return hotel;
    }

    public List<Hotel>? GetHotels(string? name = null)
    {
        return [.. _db.Hotels.Where(p => name == null || p.Name.Contains(name))];
    }
}
