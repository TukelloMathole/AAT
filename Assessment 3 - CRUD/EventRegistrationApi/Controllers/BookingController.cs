using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventRegistrationApi.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EventRegistrationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BookingController> _logger;

        public BookingController(AppDbContext context, ILogger<BookingController> logger)
        {
            _context = context;
            _logger = logger; 
        }

        [HttpPost("{eventId}/Book")]
        public async Task<IActionResult> BookEvent(int eventId, [FromForm] BookingModel bookingModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for booking request: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            // Fetch the event to book
            var eventToBook = await _context.Events.FindAsync(eventId);

            if (eventToBook == null)
            {
                _logger.LogWarning("Event with ID {EventId} not found.", eventId);
                return NotFound("Event not found.");
            }

            // Check if there are enough seats available
            if (eventToBook.AvailableSeats < bookingModel.NumberOfTickets)
            {
                _logger.LogWarning("Not enough seats available for event ID {EventId}. Requested: {RequestedSeats}, Available: {AvailableSeats}",
                    eventId, bookingModel.NumberOfTickets, eventToBook.AvailableSeats);
                return Conflict("Not enough seats available.");
            }

            // Check if the user has already registered for this event
            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Email == bookingModel.Email && b.EventId == eventId);

            if (existingBooking != null)
            {
                _logger.LogWarning("User with email {Email} has already registered for event ID {EventId}.", bookingModel.Email, eventId);
                return Conflict("User has already registered for this event.");
            }

            // Update event's available seats
            eventToBook.AvailableSeats -= bookingModel.NumberOfTickets;

            // Create a new booking record
            var booking = new BookingModel
            {
                FullName = bookingModel.FullName,
                Email = bookingModel.Email,
                PhoneNumber = bookingModel.PhoneNumber,
                NumberOfTickets = bookingModel.NumberOfTickets,
                EventId = eventId,
                BookingDate = DateTime.UtcNow 
            };

            try
            {
                // Save the updated event and the new booking record
                _context.Events.Update(eventToBook);
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Booking successful for user {Email} for event ID {EventId}.", bookingModel.Email, eventId);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while saving the booking for event ID {EventId}.", eventId);
                return StatusCode(500, "An error occurred while saving the booking.");
            }

            return Ok("Booking successful");
        }
    }
}
