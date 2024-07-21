using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventRegistrationApi.Models;
using System;
using System.Threading.Tasks;

namespace EventRegistrationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("{eventId}/Book")]
        public async Task<IActionResult> BookEvent(int eventId, [FromForm] BookingModel bookingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Fetch the event to book
            var eventToBook = await _context.Events.FindAsync(eventId);

            if (eventToBook == null)
            {
                return NotFound("Event not found."); // 404 Not Found
            }

            // Check if there are enough seats available
            if (eventToBook.AvailableSeats < bookingModel.NumberOfTickets)
            {
                return Conflict("Not enough seats available."); // 409 Conflict
            }

            // Check if the user has already registered for this event
            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Email == bookingModel.Email && b.EventId == eventId);

            if (existingBooking != null)
            {
                return Conflict("User has already registered for this event."); // 409 Conflict
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
                BookingDate = DateTime.UtcNow // Set the booking date to current time
            };

            try
            {
                // Save the updated event and the new booking record
                _context.Events.Update(eventToBook);
                _context.Bookings.Add(booking); // Ensure `Bookings` DbSet is defined in AppDbContext
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception here
                return StatusCode(500, "An error occurred while saving the booking."); // 500 Internal Server Error
            }

            return Ok("Booking successful"); // 200 OK
        }
    }
}
