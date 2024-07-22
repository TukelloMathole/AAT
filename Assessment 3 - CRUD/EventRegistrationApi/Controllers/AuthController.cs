using Microsoft.AspNetCore.Mvc;
using EventRegistrationApi.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace EventRegistrationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto loginRequest)
        {
            if (IsValidUser(loginRequest.email, loginRequest.password))
            {
                _logger.LogInformation("User {Email} logged in successfully.", loginRequest.email);
                return Ok("Login successful!");
            }

            _logger.LogWarning("Invalid login attempt for user {Email}.", loginRequest.email);
            return Unauthorized("Invalid username or password.");
        }

        private bool IsValidUser(string email, string password)
        {
            var user = _context.AdminUsers.SingleOrDefault(u => u.Email == email && u.Password == password);
            return user != null;
        }

        [HttpGet("users")]
        public ActionResult<IEnumerable<AdminUser>> GetAllUsers()
        {
            _logger.LogInformation("Fetching all users.");
            var users = _context.AdminUsers.ToList();
            return Ok(users);
        }

        [HttpPost("users")]
        public IActionResult CreateUser([FromBody] CreateUserDto createUserDto)
        {
            // Check if the user already exists
            if (_context.AdminUsers.Any(u => u.Email == createUserDto.Email))
            {
                _logger.LogWarning("Attempt to create a user that already exists: {Email}.", createUserDto.Email);
                return BadRequest("User already exists.");
            }

            var newUser = new AdminUser
            {
                Email = createUserDto.Email,
                Password = createUserDto.Password
            };

            _context.AdminUsers.Add(newUser);
            _context.SaveChanges();

            _logger.LogInformation("User {Email} created successfully.", createUserDto.Email);
            return Ok("User created successfully.");
        }
    }
}
