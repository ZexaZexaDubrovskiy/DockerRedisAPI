using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IDistributedCache _cache;

    public ArticlesController(ApplicationDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpGet("article/{id}")]
    public async Task<ActionResult<Article>> GetArticleById(int id)
    {
        var cacheKey = $"article_{id}";
        var cachedArticle = await _cache.GetStringAsync(cacheKey);

        if (cachedArticle != null)
        {
            var articleFromCache = JsonSerializer.Deserialize<Article>(cachedArticle);
            return Ok(articleFromCache);
        }

        var article = await _context.Articles.Include(a => a.Comments)
                                             .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            return NotFound();
        }

        var serializedArticle = JsonSerializer.Serialize(article);
        await _cache.SetStringAsync(cacheKey, serializedArticle, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)  // Кэширование на 10 минут
        });

        return Ok(article);
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
