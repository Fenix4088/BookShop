using System;
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
    

    public async Task<BookRatingEntity> GetByIdAsync(int entityId, Guid userId)
    {
        return await context.BookRatings.FirstOrDefaultAsync(x => x.BookId == entityId && x.UserId == userId);
    }
}