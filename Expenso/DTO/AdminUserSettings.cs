namespace MonthlyExpenseTracker.DTO;

public class AdminUserSettings
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } =  string.Empty;
    public string City { get; set; } =  string.Empty;
    public string Country { get; set; } =  string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } =  string.Empty;
}
