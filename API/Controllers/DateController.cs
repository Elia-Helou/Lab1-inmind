using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Lab1.API.Controllers
{
    [ApiController]
    [Route("api/Date")]
    public class DateController : ControllerBase
    {
        [HttpGet("current-date")]
        public IActionResult GetCurrentDate()
        {
            var language = Request.Headers["Accept-Language"].ToString();

            var culture = language switch {
                "en-US" => new CultureInfo("en-US"),
                "es-ES" => new CultureInfo("es-ES"),
                "fr-FR" => new CultureInfo("fr-FR"),
                _ => new CultureInfo("en-US")
            };

            var currentDate = DateTime.Now.ToString("D", culture);

            return Ok(new { CurrentDate = currentDate });

        }
    }
}
