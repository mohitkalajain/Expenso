namespace MonthlyExpenseTracker.DTO
{
    public class ExpenseDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public required string PaymentMethod { get; set; }
        public required string Note { get; set; }
        public int CategoryId { get; set; }
    }
}
