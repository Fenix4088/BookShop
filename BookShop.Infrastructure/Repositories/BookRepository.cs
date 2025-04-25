using System.Collections.Generic;
using System.Linq;
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

    public IEnumerable<BookEntity> GetAllBooks(bool includeDeleted = false)
    {
        var books = context.Books;
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
        context.Books.Update(bookEntity);
    }
}