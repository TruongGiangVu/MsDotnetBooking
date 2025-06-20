namespace Core.Dtos;

public class HotelDto
{
    public string Name { get; set; } = string.Empty;
    public string RoomCode { get; set; } = string.Empty;
    public int HumanAmount { get; set; } = 0;
    public double Price { get; set; } = 0;
    public bool IsVip { get; set; } = false;
    public string? Description { get; set; } = string.Empty;
    public DateTime? OpenDate { get; set; } = DateTime.Now;
}

public class CreateHotelDto : HotelDto
{

}
public class UpdateHotelDto : HotelDto
{
    public string Id { get; set; } = string.Empty;
}
