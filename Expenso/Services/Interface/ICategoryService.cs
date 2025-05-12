using MonthlyExpenseTracker.DTO;

namespace MonthlyExpenseTracker.Services.Interface
{
    public interface ICategoryService
    {
        Task<CategoryDTO> GetByIdAsync(int id);
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task AddAsync(CategoryDTO category);
        Task UpdateAsync(CategoryDTO category);
        Task DeleteAsync(int id);
    }
}
