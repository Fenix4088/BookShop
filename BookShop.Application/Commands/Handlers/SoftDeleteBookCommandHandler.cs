using BookShop.Application.Abstractions;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Commands.Handlers;

public class SoftDeleteBookCommandHandler: ICommandHandler<SoftDeleteBookCommand>
{
    private readonly IBookRepository _bookRepository;

    public SoftDeleteBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task Handler(SoftDeleteBookCommand command)
    {
        var book = await _bookRepository.GetBookById(command.BookId);

        if (book is null)
        {
            throw new BookNotFoundException(command.BookId);
        }
        
        
        _bookRepository.SoftRemove(book);
        await _bookRepository.SaveAsync();
    }
}