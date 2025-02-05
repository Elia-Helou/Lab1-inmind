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
            try
            {
                var language = Request.Headers["Accept-Language"].ToString();

                var culture = language switch
                {
                    "en-US" => new CultureInfo("en-US"),
                    "es-ES" => new CultureInfo("es-ES"),
                    "fr-FR" => new CultureInfo("fr-FR"),
                    _ => throw new ArgumentException("Invalid language. Supported languages are en-US, es-ES, fr-FR.")
                };

                var currentDate = DateTime.Now.ToString("D", culture);

                return Ok(new { CurrentDate = currentDate });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred", Details = ex.Message });
            }
            finally
            {
                Console.WriteLine("Request processed at " + DateTime.Now);
            }
        }
    }
}
