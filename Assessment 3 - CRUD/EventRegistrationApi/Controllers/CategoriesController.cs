using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventRegistrationApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EventRegistrationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(AppDbContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategories()
        {
            _logger.LogInformation("Fetching all categories.");
            var categories = await _context.Categories.ToListAsync();
            _logger.LogInformation("Successfully fetched {Count} categories.", categories.Count);
            return categories;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetCategory(int id)
        {
            _logger.LogInformation("Fetching category with ID {Id}.", id);
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category with ID {Id} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully fetched category with ID {Id}.", id);
            return category;
        }

        [HttpPost]
        public async Task<ActionResult<CategoryModel>> PostCategory(CategoryModel category)
        {
            _logger.LogInformation("Creating a new category with name {CategoryName}.", category.CategoryName);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully created category with ID {Id}.", category.CategoryId);
            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryModel category)
        {
            if (id != category.CategoryId)
            {
                _logger.LogWarning("Mismatch between ID in URL ({Id}) and ID in body ({CategoryId}).", id, category.CategoryId);
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully updated category with ID {Id}.", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    _logger.LogWarning("Category with ID {Id} not found during update.", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError("Error occurred while updating category with ID {Id}.", id);
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            _logger.LogInformation("Deleting category with ID {Id}.", id);
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {Id} not found during delete.", id);
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted category with ID {Id}.", id);
            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
