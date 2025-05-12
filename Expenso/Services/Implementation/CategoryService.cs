using Microsoft.EntityFrameworkCore;
using MonthlyExpenseTracker.Data;
using MonthlyExpenseTracker.DTO;
using MonthlyExpenseTracker.EntityModels;
using MonthlyExpenseTracker.Helper.Mapper;
using MonthlyExpenseTracker.Services.Interface;

namespace MonthlyExpenseTracker.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _xpensoContext;
        public CategoryService(AppDbContext xpensoContext)
        {
            _xpensoContext = xpensoContext;
        }
        public async Task AddAsync(CategoryDTO categoryDto)
        {
            // Map DTO to entity
            var category = Mapper.Map<CategoryDTO, Category>(categoryDto);

            // Add the Category to the database
            await _xpensoContext.Categories.AddAsync(category);
            await _xpensoContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _xpensoContext.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            // Remove the Category from the database
            _xpensoContext.Categories.Remove(category);
            await _xpensoContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var category = await _xpensoContext.Categories.ToListAsync();

            // Map list of entities to list of DTOs
            var categoryDto = Mapper.MapList<Category, CategoryDTO>(category);
            return categoryDto;
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var category = await _xpensoContext.Categories.FindAsync(id);
            if (category is null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            // Map entity to DTO

            var categoryDto = Mapper.Map<Category, CategoryDTO>(category);
            return categoryDto;
        }

        public async Task UpdateAsync(CategoryDTO categoryDto)
        {
            var category = await _xpensoContext.Categories.FindAsync(categoryDto.Id);
            if (category is null)
            {
                throw new KeyNotFoundException($"Category with ID {categoryDto.Id} not found.");
            }

            // Map DTO to entity and update properties
            Mapper.Map(categoryDto, category);

            // Update the Category in the database
            await _xpensoContext.SaveChangesAsync();
        }
    }
}
