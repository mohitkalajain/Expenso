using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonthlyExpenseTracker.DTO;
using MonthlyExpenseTracker.Helper.ResponseVM;
using MonthlyExpenseTracker.Helper.Validator;
using MonthlyExpenseTracker.Services.Interface;

namespace MonthlyExpenseTracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            try
            {
                var result = await _expenseService.GetByIdAsync(id);
                var response = new ResponseVm<ExpenseDTO>(result, "Expense fetched successfully", 200);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var response = new ResponseVm<ExpenseDTO>(ex.Message, "Expense not found", 404);
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseVm<ExpenseDTO>(ex.Message, "An error occurred", 500);
                return StatusCode(500, response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenses()
        {
            try
            {
                var result = await _expenseService.GetAllAsync();
                var response = new ResponseVm<IEnumerable<ExpenseDTO>>(result, "Expenses fetched successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseVm<IEnumerable<ExpenseDTO>>(ex.Message, "Failed to fetch expenses", 500);
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseDTO expenseDto)
        {
            ExpenseValidator _addExpencevalidator = new ExpenseValidator();
            var validationResult = _addExpencevalidator.Validate(expenseDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var response = new ResponseVm<string>(errors, "Validation failed", 400);
                return BadRequest(response);
            }
            try
            {
                await _expenseService.AddAsync(expenseDto);
                var response = new ResponseVm<string>("Expense created successfully", "Success", 201);
                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                var response = new ResponseVm<string>(ex.Message, "Failed to create expense", 500);
                return StatusCode(500, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseDTO expenseDto)
        {
            try
            {
                // Attach the ID to the DTO if not already passed
                expenseDto.Id = id;

                await _expenseService.UpdateAsync(expenseDto);

                var response = new ResponseVm<string>("Expense updated successfully", "Success", 200);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var response = new ResponseVm<string>(ex.Message, "Expense not found", 404);
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseVm<string>(ex.Message, "An error occurred while updating expense", 500);
                return StatusCode(500, response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            ExpenseDeleteValidator _deleteValidator=new ExpenseDeleteValidator();
            var validationResult = _deleteValidator.Validate(id);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var response = new ResponseVm<string>(errors, "Validation failed", 400);
                return BadRequest(response);
            }

            try
            {
                await _expenseService.DeleteAsync(id);

                var response = new ResponseVm<string>("Expense deleted successfully", "Success", 200);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var response = new ResponseVm<string>(ex.Message, "Expense not found", 404);
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseVm<string>(ex.Message, "An error occurred while deleting expense", 500);
                return StatusCode(500, response);
            }
        }

        [HttpGet("monthly-summary")]
        public async Task<IActionResult> GetMonthlySummary(int year, int month)
        {
            try
            {
                // Call the service to get the summary
                var summary = await _expenseService.GetMonthlySummaryAsync(year, month);

                return Ok(summary);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
