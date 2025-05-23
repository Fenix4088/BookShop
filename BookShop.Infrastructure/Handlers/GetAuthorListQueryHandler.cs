using System.Linq;
using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Enums;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Domain;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Pagination;
using BookShop.Models.Queries;

namespace BookShop.Infrastructure.Handlers;

public class GetAuthorListQueryHandler : IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>>
{
    private readonly ShopDbContext dbContext;

    public GetAuthorListQueryHandler(ShopDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IPagedResult<AuthorModel>> Handler(GetAuthorListQuery query)
    {
        var dbQuery = dbContext.Authors.AsQueryable().Where(x => x.DeletedAt.HasValue == query.IsDeleted);
        
        if (!string.IsNullOrWhiteSpace(query.SearchByNameAndSurname))
        {
            dbQuery = dbQuery.Where(x => x.Name.Contains(query.SearchByNameAndSurname) || x.Surname.Contains(query.SearchByNameAndSurname));
        }

        IOrderedQueryable<AuthorEntity> orderedQuery = query.SortDirection == SortDirection.Descending
            ? dbQuery.OrderBy(x => x.Surname)
            : dbQuery.OrderByDescending(x => x.Surname);
        
        var pagedResult = await orderedQuery.ToPagedResult(query, x => x.ToModel());
        return pagedResult;
    }
}