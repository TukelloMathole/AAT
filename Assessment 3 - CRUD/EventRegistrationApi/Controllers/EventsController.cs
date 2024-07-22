using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventRegistrationApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EventRegistrationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EventsController> _logger;

        public EventsController(AppDbContext context, ILogger<EventsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _context.Events.ToListAsync();
            _logger.LogInformation("Fetched {Count} events from the database.", events.Count);
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                _logger.LogWarning("Event with ID {EventId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Fetched event with ID {EventId}.", id);
            return Ok(@event);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventDto dto)
        {
            _logger.LogInformation("Received CreateEvent request.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for CreateEvent request: {ModelStateErrors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            var newEvent = new Event
            {
                EventName = dto.EventName,
                EventDate = dto.EventDate,
                Location = dto.Location,
                EventTime = dto.EventTime,
                AvailableSeats = dto.AvailableSeats,
                Description = dto.Description,
                Category = dto.Category,
            };

            _logger.LogInformation("Creating Event: {@Event}", newEvent);

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Event created successfully with ID {EventId}.", newEvent.EventId);

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.EventId }, newEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromForm] CreateEventDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for UpdateEvent request: {ModelStateErrors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                _logger.LogWarning("Event with ID {EventId} not found for update.", id);
                return NotFound();
            }

            _logger.LogInformation("Updating Event with ID {EventId}: {@EventDto}", id, dto);

            @event.EventName = dto.EventName;
            @event.EventDate = dto.EventDate;
            @event.Location = dto.Location;
            @event.EventTime = dto.EventTime;
            @event.AvailableSeats = dto.AvailableSeats;
            @event.Description = dto.Description;
            @event.Category = dto.Category;

            _context.Entry(@event).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Event with ID {EventId} updated successfully.", id);

            return Ok(@event);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                _logger.LogWarning("Event with ID {EventId} not found for deletion.", id);
                return NotFound();
            }

            _logger.LogInformation("Deleting Event with ID {EventId}.", id);

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Event with ID {EventId} deleted successfully.", id);

            return Ok();
        }
    }
}
