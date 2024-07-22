using Microsoft.AspNetCore.Mvc;
using EventRegistrationApi.Models;
using System.Linq;
using System.Collections.Generic;

namespace EventRegistrationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto loginRequest)
        {
            if (IsValidUser(loginRequest.email, loginRequest.password))
            {
                return Ok("Login successful!");
            }

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
            var users = _context.AdminUsers.ToList();
            return Ok(users);
        }

        [HttpPost("users")]
        public IActionResult CreateUser([FromBody] CreateUserDto createUserDto)
        {
            // Check if the user already exists
            if (_context.AdminUsers.Any(u => u.Email == createUserDto.Email))
            {
                return BadRequest("User already exists.");
            }

            var newUser = new AdminUser
            {
                Email = createUserDto.Email,
                Password = createUserDto.Password
            };

            _context.AdminUsers.Add(newUser);
            _context.SaveChanges();

            return Ok("User created successfully.");
        }
    }
}
