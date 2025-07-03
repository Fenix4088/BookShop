using BookShop.Application.Abstractions;
using BookShop.Application.Models;
using BookShop.Domain.Repositories;
using BookShop.Shared.Pagination;
using BookShop.Shared.Pagination.Abstractions;

namespace BookShop.Application.Queries.Handlers;

public class GetAuthorListQueryHandler(IAuthorRepository authorRepository)
    : IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>>
{
    public async Task<IPagedResult<AuthorModel>> Handler(GetAuthorListQuery query)
    {
        
        var authorsPagedResult = await authorRepository.GetPagedResultAsync(query, query.SortDirection, query.SearchByNameAndSurname, query.IsDeleted);
        
        var authorModels = authorsPagedResult.Items.Select(author => author.ToModel()).ToList();
        
        var pagedResult = new PagedResultModel<AuthorModel>
        {
            Items = authorModels,
            PageSize = authorsPagedResult.PageSize,
            TotalRowCount = authorsPagedResult.TotalRowCount,
            PageCount = authorsPagedResult.PageCount,
            CurrentPage = authorsPagedResult.CurrentPage, 
            SortDirection = authorsPagedResult.SortDirection
        };
        return pagedResult;
    }
}