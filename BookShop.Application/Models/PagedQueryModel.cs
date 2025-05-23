using BookShop.Application.Enums;

namespace BookShop.Application.Models;

public class PagedQueryModel
{
    public int CurrentPage { get; set; }
    public int RowCount { get; set; }
    public SortDirection SortDirection { get; set; } = SortDirection.Descending;
    public string SearchByNameAndSurname { get; set; } = string.Empty;
    //TODO: just for admin user
    public bool IsDeleted { get; set; } = false;
}