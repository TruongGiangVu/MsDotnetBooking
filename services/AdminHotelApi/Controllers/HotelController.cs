using AdminHotelApi.Models;
using AdminHotelApi.Repositories;
using AdminHotelApi.Services;

using Microsoft.AspNetCore.Mvc;

namespace AdminHotelApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly ILogger<HotelController> _logger;
    private readonly IHotelRepository _hotelRepository;
    private readonly IHotelService _hotelService;

    public HotelController(ILogger<HotelController> logger,
                            IHotelRepository hotelRepository,
                            IHotelService hotelService)
    {
        _logger = logger;
        _hotelRepository = hotelRepository;
        _hotelService = hotelService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return Ok("AdminHotelController is running!");
    }

    [HttpGet("item")]
    [ProducesResponseType(typeof(ResponseDto<List<Hotel>>), StatusCodes.Status200OK)]
    public IActionResult GetHotel()
    {
        List<Hotel>? hotels = _hotelRepository.GetHotels();
        return Ok(hotels);
    }

    [HttpGet("item/{id}")]
    [ProducesResponseType(typeof(ResponseDto<Hotel>), StatusCodes.Status200OK)]
    public IActionResult GetHotelById(string id)
    {
        _logger.LogInformation("{method}: {id}", nameof(GetHotelById), id);

        ResponseDto<Hotel?> response = _hotelService.GetHotelById(id);

        _logger.LogInformation("{method} response: {response}", nameof(GetHotelById), response.ToJsonString());
        return Ok(response);
    }

    [HttpPost("item")]
    [ProducesResponseType(typeof(ResponseDto<Hotel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateHotelAsync(CreateHotelDto input)
    {
        _logger.LogInformation("{method}: {input}", nameof(CreateHotelAsync), input.ToJsonString());
        ResponseDto<Hotel> response = await _hotelService.CreateHotelAsync(input);
        _logger.LogInformation("{method} response: {response}", nameof(CreateHotelAsync), response.ToJsonString());
        return Ok(response);
    }

    [HttpPut("item/{id}")]
    [ProducesResponseType(typeof(ResponseDto<Hotel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateHotelAsync(string? id, UpdateHotelDto input)
    {
        _logger.LogInformation("{method}: {id}, {input}", nameof(UpdateHotelAsync), id, input.ToJsonString());
        ResponseDto<Hotel> response = await _hotelService.UpdateHotelAsync(id, input);
        _logger.LogInformation("{method} response: {response}", nameof(UpdateHotelAsync), response.ToJsonString());
        return Ok(response);
    }

    [HttpDelete("item/{id}")]
    [ProducesResponseType(typeof(ResponseDto<Hotel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteHotelAsync(string id)
    {
        _logger.LogInformation("{method}: {id}", nameof(DeleteHotelAsync), id);
        ResponseDto<Hotel> response = await _hotelService.DeleteHotelAsync(id);
        _logger.LogInformation("{method} response: {response}", nameof(DeleteHotelAsync), response.ToJsonString());
        return Ok(response);
    }
}
