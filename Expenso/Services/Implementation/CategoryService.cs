using MonthlyExpenseTracker.DTO;
using MonthlyExpenseTracker.Services.Interface;

namespace MonthlyExpenseTracker.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        public Task AddAsync(CategoryDTO category)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CategoryDTO category)
        {
            throw new NotImplementedException();
        }
    }
}
