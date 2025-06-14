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

public class BookRatingRepository : GenericRepository<BookRatingEntity, ShopDbContext>, IRatingRepository<BookRatingEntity> {
    public BookRatingRepository(ShopDbContext shopDbContext) : base(shopDbContext)
    {
    }
    

    public async Task<BookRatingEntity> GetByEntityAndUserIdsAsync(int entityId, Guid userId)
    {
        return await context.BookRatings.FirstOrDefaultAsync(x => x.BookId == entityId && x.UserId == userId);
    }

    public async Task<List<BookRatingEntity>> GetAllByEntityIdAsync(int entityId)
    {
        return await context.BookRatings.Where(x => x.BookId == entityId).ToListAsync();
    }
}