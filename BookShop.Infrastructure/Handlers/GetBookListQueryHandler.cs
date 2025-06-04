using System.Linq;
using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Enums;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Domain;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;

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
        var dbQuery = dbContext.Books.AsQueryable();
        
        dbQuery = dbQuery.Include(x => x.Author).Where(x => query.IsDeleted == x.DeletedAt.HasValue);
        
        if (!string.IsNullOrEmpty(query.SearchByBookTitle))
        {
            dbQuery = dbQuery.Where(x => x.Title.Contains(query.SearchByBookTitle));
        }
        
        if (!string.IsNullOrEmpty(query.SearchByAuthorName))
        {
            dbQuery = dbQuery.Where(x => x.Author.Name.Contains(query.SearchByAuthorName) || 
                                         x.Author.Surname.Contains(query.SearchByAuthorName));
        }
        
        IOrderedQueryable<BookEntity> orderedQuery = query.SortDirection == SortDirection.Ascending
            ? dbQuery.OrderBy(x => x.Title)
            : dbQuery.OrderByDescending(x => x.Title);

        return  await orderedQuery.ToPagedResult(query, x => x.ToModel());
    }
} 