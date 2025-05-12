using MonthlyExpenseTracker.DTO;

namespace MonthlyExpenseTracker.Services.Interface
{
    public interface IExpenseService
    {
        Task<ExpenseDTO> GetByIdAsync(int id);
        Task<List<ExpenseDTO>> GetAllAsync();
        Task AddAsync(ExpenseDTO expense);
        Task UpdateAsync(ExpenseDTO expense);
        Task DeleteAsync(int id);
        Task<List<MonthlySummaryDTO>> GetMonthlySummaryAsync(int year, int month);
    }
}
