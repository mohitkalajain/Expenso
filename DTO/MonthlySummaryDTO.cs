namespace MonthlyExpenseTracker.DTO
{
    public class MonthlySummaryDTO
    {
        public required string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
        public string? ExpenseNotes { get; set; }
    }
}
