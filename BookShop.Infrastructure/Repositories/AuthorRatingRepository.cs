using System;
using System.Collections.Generic;
using System.Linq;
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
    

    public Task<AuthorRatingEntity> GetByEntityAndUserIdsAsync(int entityId, Guid userId)
    {
        return context.AuthorRatings.FirstOrDefaultAsync(x => x.AuthorId == entityId && x.UserId == userId);
    }

    public async Task<List<AuthorRatingEntity>> GetAllByEntityIdAsync(int entityId)
    {
        return await context.AuthorRatings.Where(x => x.AuthorId == entityId).ToListAsync(); 
    }
}
