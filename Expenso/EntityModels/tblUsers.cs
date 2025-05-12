using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonthlyExpenseTracker.EntityModels;

[Table("tblUsers")]
public class tblUsers
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public int RoleId { get; set; }
    public tblRole Role { get; set; }

    public string Provider { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }

    public DateTime CreatedAt { get; set; }
}
