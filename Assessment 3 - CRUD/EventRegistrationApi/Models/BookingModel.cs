namespace EventRegistrationApi.Models
{
    public class BookingModel
    {
        public int Id { get; set; } // Unique identifier for the booking
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfTickets { get; set; }
        public int EventId { get; set; } // Links to the Event
        public DateTime BookingDate { get; set; } // Optional: to track when the booking was made
    }
}
