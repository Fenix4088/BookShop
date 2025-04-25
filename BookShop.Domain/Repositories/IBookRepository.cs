using System.Collections.Generic;

namespace BookShop.Domain.Repositories;

public interface IBookRepository: IRepository<BookEntity>
{
    public IEnumerable<BookEntity> GetAllBooks(bool includeDeleted = false);
    public IEnumerable<BookEntity> GetAllBooksByAuthor(int authorId, bool includeDeleted = false);

    public void BulkSoftRemove(int authorId, bool includeDeleted = false);

}