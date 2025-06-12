using System.Linq;
using System.Threading.Tasks;
using BookShop.Domain;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Repositories.Abstractions;
using BookShop.Shared.Enums;
using BookShop.Shared.Pagination;
using BookShop.Shared.Pagination.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repositories;

public class AuthorRepository : GenericRepository<AuthorEntity, ShopDbContext>, IAuthorRepository
{
    public AuthorRepository(ShopDbContext shopDbContext) : base(shopDbContext)
    {
    }

    public async Task<bool> IsUniqueAuthorAsync(string name, string surname)
    {
        return !(await context.Authors.Where(x => x.DeletedAt == null).AnyAsync(author =>
            author.Name == name && author.Surname == surname));
    }

    public async Task<AuthorEntity> GetById(int? id)
    {
        return await context.Authors.Include(x => x.Ratings).SingleOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<AuthorEntity> GetAllQueryable(bool isDeleted = false) =>  context.Authors
        .Include(x => x.Ratings)
        .Where(x => x.DeletedAt.HasValue == isDeleted);

    public async Task<IPagedResult<AuthorEntity>> GetPagedResultAsync(IPagedQuery<AuthorEntity> pagedQuery, SortDirection sortDirection = SortDirection.Descending,
        string searchByNameAndSurname = "", bool isDeleted = false)
    {
        var dbQuery =  GetAllQueryable(isDeleted);
        
        if (!string.IsNullOrWhiteSpace(searchByNameAndSurname))
        {
            dbQuery = dbQuery.Where(x => (x.Surname + " " + x.Name).Contains(searchByNameAndSurname));
        }

        var orderedQuery = sortDirection == SortDirection.Descending
            ? dbQuery.OrderBy(x => x.Surname)
            : dbQuery.OrderByDescending(x => x.Surname);

        
        var pagedResult = await orderedQuery.ToPagedResult(pagedQuery, x => x);
        return pagedResult;
    }

    public void SoftRemove(AuthorEntity authorEntity)
    { 
        authorEntity.SoftDelete();
        context.Authors.Update(authorEntity);
    }
}