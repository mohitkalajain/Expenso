using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonthlyExpenseTracker.Data;
using MonthlyExpenseTracker.EntityModels;

namespace MonthlyExpenseTracker.Controllers
{
    //
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get() => _context.Categories.ToList();

        [HttpPost]
        public ActionResult<Category> Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }
    }
}
