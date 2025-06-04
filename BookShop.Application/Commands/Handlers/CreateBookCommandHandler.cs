using BookShop.Application.Abstractions;
using BookShop.Domain;
using BookShop.Domain.Entities;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace BookShop.Application.Commands.Handlers;

public sealed class CreateBookCommandHandler: ICommandHandler<CreateBookCommand>
{

    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IValidator<CreateBookCommand> _validator;
    
    public CreateBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IValidator<CreateBookCommand> validator)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _validator = validator;
    }

    public async Task Handler(CreateBookCommand command)
    {

        await _validator.ValidateAndThrowAsync(command);
        
        var authorEntity = await _authorRepository.GetById(command.AuthorId);

        if (authorEntity is null)
        {
            throw new AuthorNotFoundException(command.AuthorId);
        }
        
        if(!await _bookRepository.IsUniqueBookAsync(command.Title, command.ReleaseDate))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new ("", $"Author {authorEntity.ToModel().NameAndSurname} already has a book with the title {command.Title} and release date: {command.ReleaseDate.Date.ToShortDateString()}.")
            }); 
        }

        var newBookEntity = BookEntity.Create(command.Title, command.Description, command.ReleaseDate, command.AuthorId);
        authorEntity.AddBook();

        await _authorRepository.UpdateAsync(authorEntity);
        await _bookRepository.AddAsync(newBookEntity);

        await _authorRepository.SaveAsync();
        await _bookRepository.SaveAsync();
    }
}