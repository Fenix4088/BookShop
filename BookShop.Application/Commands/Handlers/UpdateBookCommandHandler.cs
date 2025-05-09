using BookShop.Application.Abstractions;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Commands.Handlers;

public class UpdateBookCommandHandler: ICommandHandler<UpdateBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;

    public UpdateBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    public async Task Handler(UpdateBookCommand command)
    {
        var book = await _bookRepository.GetBookById(command.Id);

        if (book is null) throw new BookNotFoundException(command.Id);
        
        book.Update(command.AuthorId, command.Title, command.Description, command.ReleaseDate);
    }
}