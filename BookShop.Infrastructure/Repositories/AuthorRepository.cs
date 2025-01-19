using System.Threading.Tasks;
using BookShop.Domain;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repositories;

public class AuthorRepository : GenericRepository<AuthorEntity>, IAuthorRepository
{
    private readonly ShopDbContext _shopDbContext;
    public AuthorRepository(ShopDbContext shopDbContext) : base(shopDbContext)
    {
        _shopDbContext = shopDbContext;
    }

    public async Task<bool> IsUniqueAuthorAsync(string name, string surname)
    {
        return !(await _shopDbContext.Authors.AnyAsync(author =>
            author.Name == name && author.Surname == surname));
    }
}