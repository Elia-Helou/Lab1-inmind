using Microsoft.AspNetCore.Mvc;

namespace Lab1.API.Controllers
{
    [ApiController]
    [Route("api/Upload")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Ensure the file is an image
            if (!file.ContentType.StartsWith("image/"))
            {
                return BadRequest("Uploaded file is not an image.");
            }

            // Create a unique file name to avoid overwriting existing files
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Get the path to the wwwroot directory
            var wwwrootPath = _env.WebRootPath;

            // Combine the wwwroot path with the "uploads" directory and the file name
            var filePath = Path.Combine(wwwrootPath, "uploads", fileName);

            // Ensure the "uploads" directory exists
            var uploadsDir = Path.Combine(wwwrootPath, "uploads");
            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            // Save the file to the uploads directory
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the file path or a success message
            return Ok(new { FilePath = filePath, Message = "File uploaded successfully." });
        }
    }
}
