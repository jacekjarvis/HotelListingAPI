using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{
    public class HotelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
