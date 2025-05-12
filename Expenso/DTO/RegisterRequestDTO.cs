namespace MonthlyExpenseTracker.DTO;

public class RegisterRequestDTO
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }

    public required string City { get; set; }
    public required string Country { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Gender { get; set; } // "Male", "Female", "Other"
}
