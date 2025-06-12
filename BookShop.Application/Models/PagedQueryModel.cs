
using BookShop.Shared.Enums;

namespace BookShop.Application.Models;

public class PagedQueryModel
{
    public int CurrentPage { get; set; }
    public int RowCount { get; set; }
    public SortDirection SortDirection { get; set; } = SortDirection.Descending;
    public bool IsDeleted { get; set; } = false;
}