using System;
using System.Threading.Tasks;
using BookShop.Domain.Entities.Rating;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repositories;

public class AuthorRatingRepository : GenericRepository<AuthorRatingEntity, ShopDbContext>, IRatingRepository<AuthorRatingEntity>
{
    public AuthorRatingRepository(ShopDbContext shopDbContext) : base(shopDbContext)
    {
    }
    

    public Task<AuthorRatingEntity> GetByIdAsync(int entityId, Guid userId)
    {
        return context.AuthorRatings.FirstOrDefaultAsync(x => x.AuthorId == entityId && x.UserId == userId);
    }
}
