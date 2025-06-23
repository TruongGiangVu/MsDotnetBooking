using CustomerHotelApi.Models;

using Microsoft.AspNetCore.Mvc;

using OpenSearch.Client;

namespace CustomerHotelApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly ILogger<HotelController> _logger;
    private readonly IOpenSearchClient _openSearchClient;

    public HotelController(ILogger<HotelController> logger, IOpenSearchClient openSearchClient)
    {
        _logger = logger;
        _openSearchClient = openSearchClient;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return Ok("CustomerHotelController is running!");
    }

    [HttpGet("open-search/{id}")]
    [ProducesResponseType(typeof(Hotel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOpenSearchHotelById(string id)
    {
        _logger.LogInformation("Fetching hotel with ID: {Id}", id);
        var response = await _openSearchClient.GetAsync<Hotel>(id);

        if (!response.Found)
            return NotFound();

        return Ok(response.Source);
    }

    [HttpGet("open-search")]
    [ProducesResponseType(typeof(IEnumerable<Hotel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchOpenSearch([FromQuery] string? name)
    {
        _logger.LogInformation("Searching hotels with name: {Name}", name);
        var searchResponse = await _openSearchClient.SearchAsync<Hotel>(s => s
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Name)
                    .Query(query: name)
                )
            )
        );

        return Ok(searchResponse.Documents);
    }
}
