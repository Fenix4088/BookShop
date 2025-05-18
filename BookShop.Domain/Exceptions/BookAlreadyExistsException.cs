namespace BookShop.Domain.Exceptions;

public class BookAlreadyExistsException: BookShopException
{
    public BookAlreadyExistsException(string authorNameAndSurname, string bookTitle) : base($"Author {authorNameAndSurname} already has a book with the title {bookTitle}.")
    {
    }
}