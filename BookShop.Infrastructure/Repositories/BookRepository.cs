using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Domain;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repositories;

public class BookRepository: GenericRepository<BookEntity, ShopDbContext>, IBookRepository
{
    public BookRepository(ShopDbContext shopDbContext) : base(shopDbContext)
    {
    }

    public Task<BookEntity> GetBookById(int bookId) => context.Books.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == bookId);

    public IEnumerable<BookEntity> GetAllBooks(bool includeDeleted = false)
    {
        var books = context.Books.Include(x => x.Author);
        return includeDeleted ? books.ToList() : books.Where(x => x.DeletedAt == null).ToList();
    }


    public IEnumerable<BookEntity> GetAllBooksByAuthor(int authorId, bool includeDeleted = false)
    {
        return GetAllBooks(includeDeleted).Where(x => x.AuthorId == authorId);
    }

    public void BulkSoftRemove(int authorId, bool includeDeleted = false)
    {
        var books = GetAllBooksByAuthor(authorId, includeDeleted);

        foreach (var bookEntity in books)
        {
            SoftRemove(bookEntity);
        }
    }


    public void SoftRemove(BookEntity bookEntity)
    { 
        bookEntity.Delete();
        bookEntity.Author.RemoveBook();
        context.Books.Update(bookEntity);
    }
}