using System;
using System.Threading.Tasks;
using BookShop.Application.Users;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Identity;
using BookShop.Infrastructure.Repositories.Abstractions;
using BookShop.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repositories;

public sealed  class UserRepository : GenericRepository<BookShopUser, ShopDbContext>, IUserRepository
{
    public UserRepository(ShopDbContext shopDbContext) : base(shopDbContext)
    {
    }

    public async Task<BookShopUserModel?> GetByEmailAsync(string email)
    {
        return (await context.Users.FirstOrDefaultAsync(x => x.Email == email))?.ToModel();
    }

    public async Task<BookShopUserModel?> GetByIdAsync(Guid id)
    {
        return (await context.Users
            .Include(x => x.Ratings)
            .Include(x => x.RatingsAuthor)
            .FirstOrDefaultAsync(x => x.Id == id))?.ToModel();
    }
}