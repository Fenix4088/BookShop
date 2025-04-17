using BookShop.Application.Abstractions;
using BookShop.Domain;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Commands.Handlers;

public sealed class CreateBookCommandHandler: ICommandHandler<CreateBookCommand>
{

    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    
    public CreateBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    public async Task Handler(CreateBookCommand command)
    {
        var authorEntity = await _authorRepository.GetById(command.AuthorId);

        if (authorEntity is null)
        {
            throw new AuthorNotFoundException(command.AuthorId);
        }

        var newBookEntity = BookEntity.Create(command.Title, command.Description, command.ReleaseDate, command.AuthorId,
            command.CoverImgUrl);
        authorEntity.AddBook();

        await _authorRepository.UpdateAsync(authorEntity);
        await _bookRepository.AddAsync(newBookEntity);

        await _authorRepository.SaveAsync();
        await _bookRepository.SaveAsync();
    }
}