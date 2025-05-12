using Microsoft.EntityFrameworkCore;
using MonthlyExpenseTracker.Data;
using MonthlyExpenseTracker.DTO;
using MonthlyExpenseTracker.EntityModels;
using MonthlyExpenseTracker.Helper.Mapper;
using MonthlyExpenseTracker.Services.Interface;

namespace MonthlyExpenseTracker.Services.Implementation
{
    public class ExpenseService : IExpenseService
    {
        private readonly AppDbContext _context;

        public ExpenseService( AppDbContext context)
        {
           _context = context;
        }

        public async Task<ExpenseDTO> GetByIdAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                throw new KeyNotFoundException($"Expense with ID {id} not found.");
            }

            // Map entity to DTO

            var expenseDto = Mapper.Map<Expense, ExpenseDTO>(expense);
            return expenseDto;
        }
        public async Task<List<ExpenseDTO>> GetAllAsync()
        {
            var expenses = await _context.Expenses.ToListAsync();

            // Map list of entities to list of DTOs
            var expenseDtos = Mapper.MapList<Expense, ExpenseDTO>(expenses);
            return expenseDtos;
        }

        public async Task AddAsync(ExpenseDTO expenseDto)
        {
            // Map DTO to entity
            var expense = Mapper.Map<ExpenseDTO, Expense>(expenseDto);

            // Add the expense to the database
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ExpenseDTO expenseDto)
        {
            var expense = await _context.Expenses.FindAsync(expenseDto.Id);
            if (expense == null)
            {
                throw new KeyNotFoundException($"Expense with ID {expenseDto.Id} not found.");
            }

            // Map DTO to entity and update properties
            Mapper.Map(expenseDto, expense);

            // Update the expense in the database
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                throw new KeyNotFoundException($"Expense with ID {id} not found.");
            }

            // Remove the expense from the database
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MonthlySummaryDTO>> GetMonthlySummaryAsync(int year, int month)
        {
            // Get total expenses for the given month and year, grouped by category
            var summary = await _context.Expenses
                .Where(e => e.Date.Year == year && e.Date.Month == month)
                .GroupBy(e => e.Category.Name)
                .Select(g => new MonthlySummaryDTO
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(e => e.Amount),
                    ExpenseNotes = g.Select(e => e.Note).FirstOrDefault() // Get the first note in the group
                })
                .ToListAsync();

            if (summary.Count == 0)
            {
                throw new KeyNotFoundException("No expenses found for the given month and year.");
            }

            return summary;
        }
    }
}
