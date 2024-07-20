using Microsoft.AspNetCore.Mvc;

namespace EventRegistrationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private const string email = "user@event.co.za";
        private const string ValidPassword = "password";

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto loginRequest)
        {
            // Check if the provided credentials match the dummy values
            if (loginRequest.email == email && loginRequest.password == ValidPassword)
            {
                // Return 200 OK with a plain text message
                return Ok("Login successful!");
            }

            // Return 401 Unauthorized with a plain text message
            return Unauthorized("Invalid username or password.");
        }
    }
}
