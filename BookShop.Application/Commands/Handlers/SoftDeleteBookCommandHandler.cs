using BookShop.Application.Abstractions;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Commands.Handlers;

public class SoftDeleteBookCommandHandler(IBookRepository bookRepository) : ICommandHandler<SoftDeleteBookCommand>
{
    public async Task Handler(SoftDeleteBookCommand command)
    {
        var book = await bookRepository.GetBookById(command.BookId);

        if (book is null)
        {
            throw new BookNotFoundException(command.BookId);
        }
        
        
        bookRepository.SoftDelete(book);
        await bookRepository.SaveAsync();
    }
}