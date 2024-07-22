using System.ComponentModel.DataAnnotations;

public class EventModel
{
    public int EventId { get; set; }

    [Required(ErrorMessage = "Event Name is required.")]
    [StringLength(100, ErrorMessage = "Event Name can't be longer than 100 characters.")]
    public string EventName { get; set; }

    [Required(ErrorMessage = "Event Date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateTime EventDate { get; set; }

    [Required(ErrorMessage = "Event Time is required.")]
    [DataType(DataType.Time, ErrorMessage = "Invalid time format.")]
    public string EventTime { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(200, ErrorMessage = "Location can't be longer than 200 characters.")]
    public string Location { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public string Category { get; set; }

    [StringLength(1000, ErrorMessage = "Description can't be longer than 1000 characters.")]
    public string Description { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Available Seats must be a positive number.")]
    public int AvailableSeats { get; set; }
}
