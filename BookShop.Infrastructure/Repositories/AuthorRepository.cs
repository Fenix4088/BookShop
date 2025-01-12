using System.Threading.Tasks;
using BookShop.Application.Commands;
using BookShop.Domain;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repositories;

public class AuthorRepository : GenericRepository<AuthorEntity>
{
    private readonly ShopDbContext _shopDbContext;
    public AuthorRepository(ShopDbContext shopDbContext) : base(shopDbContext)
    {
        _shopDbContext = shopDbContext;
    }

    // public async Task<bool> IsUniqueAuthorAsync(CreateAuthorCommand command)
    // {
    //     return await _shopDbContext.Authors.AnyAsync(author =>
    //         author.Name == command.Name && author.Surname == command.Surname);
    // }
}