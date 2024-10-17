using System.ComponentModel.DataAnnotations.Schema;

[Table("comments")]  // Указание имени таблицы в нижнем регистре
public class Comment
{
    [Column("id")]
    public int Id { get; set; }

    [Column("text")]
    public string Text { get; set; }

    [Column("rating")]
    public int Rating { get; set; }

    [Column("article_id")]
    public int ArticleId { get; set; }

    public Article Article { get; set; }
}
