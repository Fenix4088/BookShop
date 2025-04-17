namespace BookShop.Application.Models;

public class BookModel
{
    public int Id { get;  set; }
    public string Title { get;  set; }
    public string Description { get;  set; }
    public DateTime ReleaseDate { get;  set; }
    public int AuthorId { get;  set; }
    public string? CoverImgUrl { get;  set; }
}