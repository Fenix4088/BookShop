using System.Linq;
using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Pagination;

namespace BookShop.Infrastructure.Handlers;

public class GetBookListQueryHandler: IQueryHandler<GetBookListQuery, IPagedResult<BookModel>>
{

    private readonly ShopDbContext dbContext;

    public GetBookListQueryHandler(ShopDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async  Task<IPagedResult<BookModel>> Handler(GetBookListQuery query)
    {
        var dbQuery =  dbContext.Books.AsQueryable().Where(x => x.DeletedAt != null).OrderBy(x => x.CreatedAt);

        return  await dbQuery.ToPagedResult(query, x => new BookModel()
        {
            Id = x.Id,
            AuthorId = x.AuthorId,
            Description = x.Description,
            Title = x.Title,
            ReleaseDate = x.ReleaseDate,
            CoverImgUrl = x.CoverImgUrl
        });
    }
} 