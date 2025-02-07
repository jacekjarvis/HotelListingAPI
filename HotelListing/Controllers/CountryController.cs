using AutoMapper;
using HotelListing.Data;
using HotelListing.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Controllers;

[ApiController] //Necessary
[Route("api/[controller]")] //Necessary
public class CountryController : ControllerBase
{
    private readonly ILogger<CountryController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CountryController(ILogger<CountryController> logger, ApplicationDbContext db, IMapper mapper)
    {
        _logger = logger;
        _db = db;
        _mapper = mapper;
    }

    [HttpGet] //Necessary
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountries()
    {
        try
        { 
            var countries = await _db.Countries.ToListAsync();
            var results = _mapper.Map<List<CountryDTO>>(countries);

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
            return StatusCode(500, "Internal Server Error. Please try again later");
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountry(int id)
    {
        try
        { 
            var country = await _db.Countries
                .Include(c=>c.Hotels)
                .FirstOrDefaultAsync(c => c.Id == id);

            var results = _mapper.Map<CountryDTO>(country);

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong n the {nameof(GetCountry)}");
            return StatusCode(500, "Internal Server Error. Please try again later");
        }
    }
}
