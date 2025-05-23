namespace BookShop.Application.Models;

public class PageBookQueryModel : PagedQueryModel
{
    public string SearchByBookTitle { get; set; } = string.Empty;
    public string SearchByAuthorName { get; set; } = string.Empty;

}