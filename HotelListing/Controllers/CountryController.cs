using AutoMapper;
using HotelListing.Data;
using HotelListing.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Controllers;

[Route("api/[controller]")] //Necessary
[ApiController] //Necessary
public class CountryController : Controller
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
    public async Task<IActionResult> GetCountries()
    {
        var countries = await _db.Countries.ToListAsync();
        var results = _mapper.Map<List<CountryDTO>>(countries);

        return Ok(results);
    }

    [HttpGet("{id:int}")] 
    public async Task<IActionResult> GetCountry(int id)
    {
        var country = await _db.Countries
            .Include(c=>c.Hotels)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (country == null)
        {
            return NotFound(); // Return 404 if the country is not found
        }
        var results = _mapper.Map<CountryDTO>(country);

        return Ok(results);
    }
}
