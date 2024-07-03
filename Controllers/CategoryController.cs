using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase{
    private readonly AppDbContext _context;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(AppDbContext context, ILogger<CategoryController> logger){
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories(){
        _logger.LogInformation("GET /api/category called");
        return await _context.Categories.ToListAsync();
    }

    [HttpPost]
    [Authorize (Roles = "Admin")]
    public async Task<ActionResult<Category>> PostCategory(Category category){
        _logger.LogInformation("POST /api/category called");
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Category created with ID: {Id}", category.Id);
        return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> PutCategory(int id, Category category){
        if (id != category.Id)
        {
            return BadRequest();
        }

        _context.Entry(category).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    private bool CategoryExists(int id){
        return _context.Categories.Any(e => e.Id == id);
    }

    [HttpDelete("{id}")]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory(int id){
        var category = await _context.Categories.FindAsync(id);
        if (category == null){
            return NotFound();
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
        
}