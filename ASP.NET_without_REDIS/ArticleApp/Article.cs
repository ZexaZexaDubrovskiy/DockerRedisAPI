using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

[Table("articles")]
public class Article
{
    [Column("id")]  // Указание столбца "id"
    public int Id { get; set; }

    [Column("title")]  // Указание столбца "title"
    public string Title { get; set; }

    [Column("text")]  // Указание столбца "text"
    public string Text { get; set; }

    public List<Comment> Comments { get; set; }  // Связь с комментариями
}
