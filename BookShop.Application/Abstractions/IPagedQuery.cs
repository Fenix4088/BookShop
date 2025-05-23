using BookShop.Application.Enums;
using BookShop.Models.Queries.Abstractions;

namespace BookShop.Application.Abstractions;

public interface IPagedQuery<TResult> : IQuery<IPagedResult<TResult>>
{
    int CurrentPage { get; }
    int RowCount { get; }
        
    SortDirection SortDirection { get; }
}