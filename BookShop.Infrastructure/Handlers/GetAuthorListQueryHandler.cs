using System.Linq;
using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Models;
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
        var dbQuery = dbContext.Authors.AsQueryable().Where(author => author.DeletedAt == null).OrderBy(x => x.CreatedAt);
        
        var pagedResult = await dbQuery.ToPagedResult(query, x => x.ToModel());
        return pagedResult;
    }
}