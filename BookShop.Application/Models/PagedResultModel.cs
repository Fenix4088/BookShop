using BookShop.Application.Abstractions;
using BookShop.Application.Enums;

namespace BookShop.Application.Models;

public class PagedResultModel<TResult> : IPagedResult<TResult>
{
    public IEnumerable<TResult> Items { get; set; }

    public int CurrentPage { get; set; }

    public int PageCount { get; set; }

    public int PageSize { get; set; }

    public int TotalRowCount { get; set; }
    
    public SortDirection SortDirection { get; set; } = SortDirection.Descending;
    
    public string SearchByNameAndSurname { get; set; } = string.Empty;
    
    public string SearchByBookTitle { get; set; } = string.Empty;
    public string SearchByAuthorName { get; set; } = string.Empty;
    
    public bool IsDeleted { get; set; } = false;
}