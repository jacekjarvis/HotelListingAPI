using HotelListing.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Controllers;

[Route("api/[controller]")] //Necessary
[ApiController] //Necessary
public class CountryController : Controller
{
    private readonly ILogger<CountryController> _logger;
    private readonly ApplicationDbContext _db;

    public CountryController(ILogger<CountryController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;   
    }

    [HttpGet] //Necessary
    public async Task<IActionResult> GetCountries()
    {
        var countries = _db.Countries
            //.Include(c => c.Hotels)
            .ToList(); 
        return Ok(countries);
    }
}
