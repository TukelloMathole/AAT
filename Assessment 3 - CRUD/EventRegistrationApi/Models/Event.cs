namespace EventRegistrationApi.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string EventTime { get; set; }
        public int AvailableSeats { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }

}
