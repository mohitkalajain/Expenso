using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MonthlyExpenseTracker.EntityModels
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Ignore the Expenses collection during serialization to avoid circular reference
        [JsonIgnore]
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }

}
