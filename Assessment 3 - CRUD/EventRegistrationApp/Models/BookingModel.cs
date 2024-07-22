using System.ComponentModel.DataAnnotations;

public class BookingModel
{
    [Required(ErrorMessage = "Full Name is required.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone Number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Number of Tickets is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid number of tickets.")]
    public int NumberOfTickets { get; set; }
}
