namespace BookShop.Application.Models;

public class PageAuthorQueryModel : PagedQueryModel
{
    public string SearchByNameAndSurname { get; set; } = string.Empty;
}