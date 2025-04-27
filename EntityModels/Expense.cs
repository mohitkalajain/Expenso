using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MonthlyExpenseTracker.EntityModels
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Ignore the Category property during serialization to avoid circular reference
        [JsonIgnore]
        public Category? Category { get; set; }
    }

}
