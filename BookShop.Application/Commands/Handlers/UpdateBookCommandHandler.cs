using System.ComponentModel.DataAnnotations;
using BookShop.Application.Abstractions;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;
using FluentValidation;

namespace BookShop.Application.Commands.Handlers;

public class UpdateBookCommandHandler: ICommandHandler<UpdateBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IValidator<UpdateBookCommand> _validator;

    public UpdateBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IValidator<UpdateBookCommand> validator)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _validator = validator;
    }

    public async Task Handler(UpdateBookCommand command)
    {
        await _validator.ValidateAndThrowAsync(command);
        
        var bookEntity = await _bookRepository.GetBookById(command.Id);

        if (bookEntity is null) throw new BookNotFoundException(command.Id);

        var authorEntity = await _authorRepository.GetById(command.AuthorId);

        if (authorEntity is null) throw new AuthorNotFoundException(command.AuthorId);
        
        bookEntity.Update(command.AuthorId, command.Title, command.Description, command.ReleaseDate);
    }
}