using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]

//[Authorize]
public class MovieController : ControllerBase{
    private readonly AppDbContext _context;
    public MovieController(AppDbContext context ){
        _context = context;
    }

    //[Authorize]
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies(string? search, int page = 1, int pageSize = 10){
        var query = _context.Movies.Include(m => m.MovieCategories).ThenInclude(mc => mc.Category).AsQueryable();
        if (!string.IsNullOrEmpty(search)){
            query = query.Where(m => m.Name.Contains(search));
        }
        var movies = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return Ok(new{
            TotalCount = await query.CountAsync(),
            Movies = movies
        });
    }
    //[Authorize]
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Movie>> GetMovie(int id){
        var movie = await _context.Movies.Include(m => m.MovieCategories).ThenInclude(mc => mc.Category).FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null){
            return NotFound();
        }
        return movie;
    }
    //[Authorize]
    [HttpPost]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Movie>> PostMovie(Movie movie){
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }
    //[Authorize]
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> PutMovie(int id, Movie movie){
        if (id != movie.Id){
            return BadRequest();
        }
        _context.Entry(movie).State = EntityState.Modified;
        try{
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException){
            if (!MovieExists(id)){
                return NotFound();
            } else {
                throw;
            }
        }
        return NoContent();
    }
    //[Authorize]
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMovie(int id){
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null){
            return NotFound();
        }
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    private bool MovieExists(int id){
        return _context.Movies.Any(m => m.Id == id);
    }
}