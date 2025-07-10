using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Domain;
using BookShop.Domain.Entities;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Repositories.Abstractions;
using BookShop.Shared.Enums;
using BookShop.Shared.Pagination;
using BookShop.Shared.Pagination.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repositories;

public class BookRepository(ShopDbContext shopDbContext)
    : GenericRepository<BookEntity, ShopDbContext>(shopDbContext), IBookRepository
{
    public Task<BookEntity> GetBookById(int bookId) => context.Books
        .Include(x => x.Author)
        .Include(x => x.Ratings)
        .FirstOrDefaultAsync(x => x.Id == bookId);

    public IEnumerable<BookEntity> GetAllBooks(bool includeDeleted = false)
    {
        var books = context.Books
            .Include(x => x.Author)
            .Include(x => x.Ratings);
        return includeDeleted ? books.ToList() : books.Where(x => x.DeletedAt == null).ToList();
    }


    public IEnumerable<BookEntity> GetAllBooksByAuthor(int authorId, bool includeDeleted = false)
    {
        return GetAllBooks(includeDeleted).Where(x => x.AuthorId == authorId);
    }
    
    
    public async Task<bool> IsUniqueBookAsync(string title, DateTime releaseDate)
    {
        return !(await context.Books.Where(x => x.DeletedAt == null).AnyAsync(book =>
            book.Title == title && book.ReleaseDate == releaseDate && book.DeletedAt == null));
    }
    public async Task<bool> IsUniqueBookAsync(int targetBookId, string title, DateTime releaseDate)
    {
        return !(await context.Books.Where(x => x.DeletedAt == null).AnyAsync(book =>
            targetBookId != book.Id && book.Title == title && book.ReleaseDate == releaseDate && book.DeletedAt == null));
    }

    public async Task<IPagedResult<BookEntity>> GetPagedResultAsync(IPagedQuery<BookEntity> pagedQuery, SortDirection sortDirection = SortDirection.Descending,
        string searchByBookTitle = "", string searchByAuthorName = "", bool isDeleted = false)
    {
        var dbQuery = context.Books.AsQueryable();
        
        dbQuery = dbQuery
            .Include(x => x.Author)
            .Include(x => x.Ratings)
            .Where(x => isDeleted == x.DeletedAt.HasValue);
        
        if (!string.IsNullOrEmpty(searchByBookTitle))
        {
            dbQuery = dbQuery.Where(x => x.Title.Contains(searchByBookTitle));
        }
        
        if (!string.IsNullOrEmpty(searchByAuthorName))
        {
            dbQuery = dbQuery.Where(x => x.Author.Name.Contains(searchByAuthorName) || 
                                         x.Author.Surname.Contains(searchByAuthorName) ||
                                         (x.Author.Surname + " " + x.Author.Name).Contains(searchByAuthorName));
        }
        
        IOrderedQueryable<BookEntity> orderedQuery = sortDirection == SortDirection.Ascending
            ? dbQuery.OrderBy(x => x.Title)
            : dbQuery.OrderByDescending(x => x.Title);
        
        return  await orderedQuery.ToPagedResult(pagedQuery, x => x);
    }

    public void BulkSoftDelete(int authorId, bool includeDeleted = false)
    {
        var books = GetAllBooksByAuthor(authorId, includeDeleted);

        foreach (var bookEntity in books)
        {
            SoftDelete(bookEntity);
        }
    }


    public void SoftDelete(BookEntity bookEntity)
    {
        bookEntity.SoftDeleteRatings();
        bookEntity.SoftDelete();
        bookEntity.Author.RemoveBook();
        context.Books.Update(bookEntity);
    }
}