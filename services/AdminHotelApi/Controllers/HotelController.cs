using AdminHotelApi.Repositories;
using AdminHotelApi.Services;

using Core.Models;

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
        return Ok("AdminHotelController is running!");
    }

    [HttpGet("item/{id}")]
    [ProducesResponseType(typeof(ResponseDto<Hotel>), StatusCodes.Status200OK)]
    public IActionResult GetHotelById(string id)
    {
        return Ok("AdminHotelController is running!");
    }

    [HttpPost("item")]
    [ProducesResponseType(typeof(ResponseDto<Hotel>), StatusCodes.Status200OK)]
    public IActionResult CreateHotel(CreateHotelDto input)
    {
        return Ok("AdminHotelController is running!");
    }

    [HttpPost("item/{id}")]
    [ProducesResponseType(typeof(ResponseDto<Hotel>), StatusCodes.Status200OK)]
    public IActionResult UpdateHotel(UpdateHotelDto input)
    {
        return Ok("AdminHotelController is running!");
    }

    [HttpDelete("item/{id}")]
    [ProducesResponseType(typeof(ResponseDto<Hotel>), StatusCodes.Status200OK)]
    public IActionResult DeleteHotel(string id)
    {
        return Ok("AdminHotelController is running!");
    }
}
