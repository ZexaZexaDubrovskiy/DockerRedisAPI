using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ArticlesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("randomArticle")]
    public async Task<ActionResult<Article>> GetRandomArticle()
    {
        var count = await _context.Articles.CountAsync();
        if (count == 0) return NotFound();

        var randomId = new Random().Next(1, count + 1);
        var article = await _context.Articles.Include(a => a.Comments)
                                             .FirstOrDefaultAsync(a => a.Id == randomId);
        return Ok(article);
    }

    [HttpGet("article/{id}")]
    public async Task<ActionResult<Article>> GetArticleById(int id)
    {
        var article = await _context.Articles.Include(a => a.Comments)
                                             .FirstOrDefaultAsync(a => a.Id == id);
        if (article == null)
        {
            return NotFound(); // Возвращаем NotFound, если статья не найдена
        }
        return Ok(article);
    }

    [HttpGet("testdb")]
    public async Task<IActionResult> TestDatabaseConnection()
    {
        try
        {
            var articleCount = await _context.Articles.CountAsync();
            return Ok(new { message = "Database connection is successful", articleCount });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to connect to the database", error = ex.Message });
        }
    }

}
