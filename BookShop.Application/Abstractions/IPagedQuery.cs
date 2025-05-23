using BookShop.Application.Abstractions;
using BookShop.Application.Enums;

namespace BookShop.Models.Queries.Abstractions
{
    public interface IPagedQuery<TResult> : IQuery<IPagedResult<TResult>>
    {
        int CurrentPage { get; }
        int RowCount { get; }
        
        SortDirection SortDirection { get; }
        
        string SearchByNameAndSurname { get; }
    }
}