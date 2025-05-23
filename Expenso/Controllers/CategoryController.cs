using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonthlyExpenseTracker.DTO;
using MonthlyExpenseTracker.Helper.ResponseVM;
using MonthlyExpenseTracker.Helper.Validator;
using MonthlyExpenseTracker.Services.Implementation;
using MonthlyExpenseTracker.Services.Interface;

namespace MonthlyExpenseTracker.Controllers
{
    
    [Route("/api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize]
        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var result = await _categoryService.GetByIdAsync(id);
                var response = new ResponseVm<CategoryDTO>(result, "Category fetched successfully", StatusCodes.Status200OK);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var response = new ResponseVm<CategoryDTO>(ex.Message, "Category not found", StatusCodes.Status404NotFound);
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseVm<CategoryDTO>(ex.Message, "An error occurred", StatusCodes.Status500InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [Authorize]
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]

        public async Task<IActionResult> GetAllCategory()
        {
            try
            {
                var result = await _categoryService.GetAllAsync();
                var response = new ResponseVm<IEnumerable<CategoryDTO>>(result, "Categories fetched successfully", StatusCodes.Status200OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseVm<IEnumerable<CategoryDTO>>(ex.Message, "Failed to fetch categories", StatusCodes.Status500InternalServerError);
                return StatusCode(500, response);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody] CategoryDTO categoryDto)
        {
            CategoryValidator _addCategoryValidator = new CategoryValidator();
            var validationResult = _addCategoryValidator.Validate(categoryDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var response = new ResponseVm<string>(errors, "Validation failed", StatusCodes.Status400BadRequest);
                return BadRequest(response);
            }
            try
            {
                await _categoryService.AddAsync(categoryDto);
                var response = new ResponseVm<string>("Category created successfully", "Success", StatusCodes.Status201Created);
                return StatusCode(201, response);
            }
            catch (InvalidOperationException ex)
            {
                var response = new ResponseVm<string>(ex.Message, "Category already exists", StatusCodes.Status409Conflict);
                return Conflict(response);  
            }
            catch (Exception ex)
            {
                var response = new ResponseVm<string>(ex.Message, "Failed to create category", StatusCodes.Status500InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDTO categoryDto)
        {
            try
            {
                // Attach the ID to the DTO if not already passed
                categoryDto.Id = id;

                await _categoryService.UpdateAsync(categoryDto);

                var response = new ResponseVm<string>("Category updated successfully", "Success", StatusCodes.Status200OK);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var response = new ResponseVm<string>(ex.Message, "Category not found", StatusCodes.Status404NotFound);
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseVm<string>(ex.Message, "An error occurred while updating category", StatusCodes.Status500InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(int id)
        {
            CategoryDeleteValidator _deleteValidator = new CategoryDeleteValidator();
            var validationResult = _deleteValidator.Validate(id);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var response = new ResponseVm<string>(errors, "Validation failed", StatusCodes.Status400BadRequest);
                return BadRequest(response);
            }

            try
            {
                await _categoryService.DeleteAsync(id);

                var response = new ResponseVm<string>("Category deleted successfully", "Success", StatusCodes.Status200OK);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var response = new ResponseVm<string>(ex.Message, "Category not found", StatusCodes.Status404NotFound);
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseVm<string>(ex.Message, "An error occurred while deleting category", StatusCodes.Status500InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
