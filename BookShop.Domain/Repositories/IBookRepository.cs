using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Domain.Repositories;

public interface IBookRepository: IRepository<BookEntity>
{
    public IEnumerable<BookEntity> GetAllBooks(bool includeDeleted = false);
    public IEnumerable<BookEntity> GetAllBooksByAuthor(int authorId, bool includeDeleted = false);

    public void BulkSoftRemove(int authorId, bool includeDeleted = false);

    public Task<BookEntity> GetBookById(int bookId);
    
    public Task<bool> IsUniqueBookAsync(string title, DateTime releaseDate);

    public void SoftRemove(BookEntity bookEntity);

}