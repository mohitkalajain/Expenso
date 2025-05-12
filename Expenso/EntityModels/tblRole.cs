using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonthlyExpenseTracker.EntityModels;

[Table("tblRoles")]
public class tblRole
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }

    public ICollection<tblUsers> Users { get; set; }
}
