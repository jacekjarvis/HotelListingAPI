using AutoMapper;
using HotelListing.Data;
using HotelListing.DTOs;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
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
            var countries = await _db.Countries
                .Include(c => c.Hotels)
                .ToListAsync();
            var results = _mapper.Map<List<CountryDTO>>(countries);

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
            return StatusCode(500, "Internal Server Error. Please try again later");
        }
    }

    [HttpGet("{id:int}", Name="GetCountry")]
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


    [Authorize(Roles = "Administrator")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");
            return BadRequest(ModelState);
        }

        try
        {
            var country = _mapper.Map<Country>(countryDTO);
            await _db.Countries.AddAsync(country);
            await _db.SaveChangesAsync();

            return CreatedAtRoute(nameof(GetCountry), new { id = country.Id }, country);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateCountry)}");
            return StatusCode(500, "Internal Server Error. Please Try Again Later.");
        }
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
            return BadRequest(ModelState);
        }

        try
        {
            var country = await _db.Countries.FirstOrDefaultAsync(q => q.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                return NotFound("Country not found"); // Or BadRequest("Submitted data is invalid")
            }

            _mapper.Map(countryDTO, country);
            _db.Countries.Update(country);
            await _db.SaveChangesAsync();

            return  NoContent(); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something Went wrong in the {nameof(UpdateCountry)}");
            return StatusCode(500, "Internal Server Error. Please Try Again Later.");
        }
    }


    [Authorize]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        if (id < 1)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
            return BadRequest();
        }

        try
        {
            var country = await _db.Countries.FirstOrDefaultAsync(q => q.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return NotFound("Country not found"); // Or BadRequest("Submitted data is invalid")
            }

            _db.Countries.Remove(country); // Pass the entity, not just the ID
            await _db.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteCountry)}");
            return StatusCode(500, "Internal Server Error. Please Try Again Later.");
        }
    }
}
