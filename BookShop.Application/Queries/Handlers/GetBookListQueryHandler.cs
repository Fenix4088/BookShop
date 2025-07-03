using BookShop.Application.Abstractions;
using BookShop.Application.Models;
using BookShop.Domain.Repositories;
using BookShop.Shared.Pagination;
using BookShop.Shared.Pagination.Abstractions;

namespace BookShop.Application.Queries.Handlers;

public class GetBookListQueryHandler(IBookRepository bookRepository)
    : IQueryHandler<GetBookListQuery, IPagedResult<BookModel>>
{
    public async  Task<IPagedResult<BookModel>> Handler(GetBookListQuery query)
    {
        
        var booksPagedResult = await bookRepository.GetPagedResultAsync(query, query.SortDirection, query.SearchByBookTitle, query.SearchByAuthorName, query.IsDeleted);
        
        var bookModels = booksPagedResult.Items.Select(book => book.ToModel()).ToList();
        var pagedResult = new PagedResultModel<BookModel>
        {
            Items = bookModels,
            PageSize = booksPagedResult.PageSize,
            TotalRowCount = booksPagedResult.TotalRowCount,
            PageCount = booksPagedResult.PageCount,
            CurrentPage = booksPagedResult.CurrentPage, 
            SortDirection = booksPagedResult.SortDirection
        };
        
        return pagedResult;
    }
} 