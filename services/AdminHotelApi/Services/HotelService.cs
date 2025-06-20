using AdminHotelApi.Dtos;
using AdminHotelApi.Repositories;

using Core.Helper.Exceptions;
using Core.Models;

using MassTransit;

namespace AdminHotelApi.Services;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;

    public HotelService(IHotelRepository hotelRepository,
                        IPublishEndpoint publishEndpoint)
    {
        _repository = hotelRepository;
        _publishEndpoint = publishEndpoint;
    }

    public Hotel? GetHotelById(string? id)
    {
        if (string.IsNullOrEmpty(id))
            throw new RequiredException($"Hotel id {id} không để trống");

        if (id == "aa")
            throw new ValidationException(details: [$"Hotel id {id} không hợp lệ"]);

        return _repository.GetHotelById(id);
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

        QueueDto<Hotel> queueDto = new()
        {
            Payload = data,
            Action = QueueAction.Create
        };
        await _publishEndpoint.Publish(queueDto);

        return ResponseDto<Hotel>.Success(data);
    }
}
