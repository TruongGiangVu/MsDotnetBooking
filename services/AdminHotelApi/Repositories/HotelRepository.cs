using Core.Models;

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

    public List<Hotel>? GetHotel(string name)
    {
        return _db.Hotels.Where(p => p.Name.Contains(name)).ToList();
    }
}
