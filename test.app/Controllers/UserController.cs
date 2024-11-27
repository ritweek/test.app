using Microsoft.AspNetCore.Mvc;
using test.app.Models;
using test.app.Services;

namespace test.app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IImageService _imageService;

        public UserController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("avatars")]
        public async Task<IActionResult> GetAvatars([FromQuery] string userIdentifier)
        {
            if (string.IsNullOrEmpty(userIdentifier))
                return BadRequest("User identifier cannot be empty.");

            try
            {
                ImageResponse response = await _imageService.GetImageUrl(userIdentifier);
                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error fetching data: {ex.Message}");
            }
        }
    }
}
