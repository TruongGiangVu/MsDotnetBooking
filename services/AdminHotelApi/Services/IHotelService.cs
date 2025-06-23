using AdminHotelApi.Models;

namespace AdminHotelApi.Services;

public interface IHotelService
{
    ResponseDto<Hotel?> GetHotelById(string? id);
    Task<ResponseDto<Hotel>> CreateHotelAsync(CreateHotelDto? dto);
    Task<ResponseDto<Hotel>> UpdateHotelAsync(string? id, UpdateHotelDto? dto);
    Task<ResponseDto<Hotel>> DeleteHotelAsync(string? id);
}
