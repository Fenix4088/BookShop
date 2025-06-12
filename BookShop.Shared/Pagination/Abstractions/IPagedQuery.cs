using BookShop.Shared.Abstractions;
using BookShop.Shared.Enums;

namespace BookShop.Shared.Pagination.Abstractions;

public interface IPagedQuery<TResult> : IQuery<IPagedResult<TResult>>
{
    int CurrentPage { get; }
    int RowCount { get; }
        
    SortDirection SortDirection { get; }
}