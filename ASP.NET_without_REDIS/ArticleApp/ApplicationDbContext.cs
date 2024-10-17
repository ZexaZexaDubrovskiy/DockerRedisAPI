using Microsoft.EntityFrameworkCore; // Для DbContext и DbSet

public class ApplicationDbContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
