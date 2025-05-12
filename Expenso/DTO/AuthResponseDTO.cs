namespace MonthlyExpenseTracker.DTO;

public class AuthResponseDTO
{
    public string Token { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
    // Optional additions
    public string FullName { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}
