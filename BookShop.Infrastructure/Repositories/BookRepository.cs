using BookShop.Domain;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Repositories.Abstractions;

namespace BookShop.Infrastructure.Repositories;

public class BookRepository: GenericRepository<BookEntity, ShopDbContext>, IBookRepository
{
    public BookRepository(ShopDbContext shopDbContext) : base(shopDbContext)
    {
    }
}