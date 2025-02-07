using AutoMapper;
using HotelListing.Data;
using HotelListing.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Controllers;

[ApiController] //Necessary
[Route("api/[controller]")] //Necessary
public class HotelController : ControllerBase
{
    private readonly ILogger<HotelController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public HotelController(ILogger<HotelController> logger, ApplicationDbContext db, IMapper mapper)
    {
        _logger = logger;
        _db = db;
        _mapper = mapper;
    }

    [HttpGet] //Necessary
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetHotels()
    {
        try
        { 
            var hotels = await _db.Hotels
                    .ToListAsync();
            var results = _mapper.Map<List<HotelDTO>>(hotels);

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotels)}");
            return StatusCode(500, "Internal Server Error. Please try again later");
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetHotel(int id)
    {
        try
        { 
            var hotel = await _db.Hotels
                .Include(h=>h.Country)
                .FirstOrDefaultAsync(h => h.Id == id);

            var results = _mapper.Map<HotelDTO>(hotel);

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong n the {nameof(GetHotel)}");
            return StatusCode(500, "Internal Server Error. Please try again later");
        }
    }
}
