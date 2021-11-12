using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sso.Application.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class WeatherForecastController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
