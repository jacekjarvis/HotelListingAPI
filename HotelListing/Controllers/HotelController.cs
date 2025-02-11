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

    
    [HttpGet("{id:int}", Name="GetHotel")]
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

    [Authorize(Roles ="Administrator")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
            return BadRequest(ModelState);
        }

        try
        {
            var hotel = _mapper.Map<Hotel>(hotelDTO);
            await _db.Hotels.AddAsync(hotel); 
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateHotel)}");
            return StatusCode(500, "Internal Server Error. Please Try Again Later.");
        }
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO hotelDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
            return BadRequest(ModelState);
        }

        try
        {
            var hotel = await _db.Hotels.FirstOrDefaultAsync(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                return NotFound("Hotel not found"); // Or BadRequest("Submitted data is invalid") if you prefer
            }

            _mapper.Map(hotelDTO, hotel);
            _db.Hotels.Update(hotel); // Make sure you have an Update method in your repository
            await _db.SaveChangesAsync();

            return NoContent(); // 204 No Content is standard for successful UPDATE
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateHotel)}");
            return StatusCode(500, "Internal Server Error. Please Try Again Later.");
        }
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        if (id < 1)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
            return BadRequest();
        }

        try
        {
            var hotel = await _db.Hotels.FirstOrDefaultAsync(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                return NotFound("Hotel not found"); // Or BadRequest("Submitted data is invalid")
            }

            _db.Hotels.Remove(hotel); // Pass the hotel entity to Delete
            await _db.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteHotel)}");
            return StatusCode(500, "Internal Server Error. Please Try Again Later.");
        }
    }
}
