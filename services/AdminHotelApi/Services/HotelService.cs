using AdminHotelApi.Helper.Exceptions;
using AdminHotelApi.Models;
using AdminHotelApi.Repositories;

namespace AdminHotelApi.Services;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _repository;
    private readonly IRabbitMQService _rabbitMQService;

    public HotelService(IHotelRepository hotelRepository,
                        IRabbitMQService rabbitMQService)
    {
        _repository = hotelRepository;
        _rabbitMQService = rabbitMQService;
    }

    public ResponseDto<Hotel?> GetHotelById(string? id)
    {
        if (string.IsNullOrEmpty(id))
            throw new RequiredException($"Hotel id {id} không để trống");

        if (id == "aa")
            throw new ValidationException(details: [$"Hotel id {id} không hợp lệ"]);

        Hotel? hotel = _repository.GetHotelById(id);
        return ResponseDto<Hotel?>.Success(hotel);
    }

    public async Task<ResponseDto<Hotel>> CreateHotelAsync(CreateHotelDto? dto)
    {
        if (dto is null)
            throw new RequiredException($"Hotel thông tin không bị null");

        CreateHotelDtoValidator validator = new();
        FluentValidation.Results.ValidationResult validationResult = validator.Validate(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(details: validationResult.Errors.Select(x => x.ErrorMessage).ToList());

        Hotel? data = _repository.CreateHotel(dto);

        if (data is null)
            throw new DatabaseException($"Tạo Hotel bị lỗi");

        await _rabbitMQService.PushAsync(QueueAction.Create, data);

        return ResponseDto<Hotel>.Success(data);
    }

    public async Task<ResponseDto<Hotel>> UpdateHotelAsync(string? id, UpdateHotelDto? dto)
    {
        if (dto is null)
            throw new RequiredException($"Hotel thông tin không bị null");

        if (string.IsNullOrEmpty(id))
            throw new RequiredException($"Hotel id update {id} không để trống");

        if (string.IsNullOrEmpty(dto.Id))
            throw new RequiredException($"Hotel id {dto.Id} không để trống");

        CreateHotelDtoValidator validator = new();
        FluentValidation.Results.ValidationResult validationResult = validator.Validate(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(details: validationResult.Errors.Select(x => x.ErrorMessage).ToList());

        Hotel? data = _repository.UpdateHotel(id, dto);

        if (data is null)
            throw new DatabaseException($"Cập nhật Hotel bị lỗi");

        await _rabbitMQService.PushAsync(QueueAction.Update, data);

        return ResponseDto<Hotel>.Success(data);
    }

    public async Task<ResponseDto<Hotel>> DeleteHotelAsync(string? id)
    {
        if (string.IsNullOrEmpty(id))
            throw new RequiredException($"Hotel id {id} không để trống");

        if (string.IsNullOrEmpty(id))
            throw new RequiredException($"Hotel id {id} không để trống");

        Hotel? data = _repository.DeleteHotel(id);

        if (data is null)
            throw new DatabaseException($"Xóa Hotel bị lỗi");

        await _rabbitMQService.PushAsync(QueueAction.Delete, data);

        return ResponseDto<Hotel>.Success(data);
    }
}
