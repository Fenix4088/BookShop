namespace BookShop.Domain.Exceptions;

public class BookNotFoundException: BookShopException
{
    public BookNotFoundException(int bookId) : base($"Book with id: {bookId} not found!")
    {
    }
}