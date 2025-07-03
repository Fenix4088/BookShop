using BookShop.Application.Abstractions;
using BookShop.Domain;
using BookShop.Domain.Abstractions;
using BookShop.Domain.Entities;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace BookShop.Application.Commands.Handlers;

public sealed class CreateBookCommandHandler(
    IBookRepository bookRepository,
    IAuthorRepository authorRepository,
    IValidator<CreateBookCommand> validator,
    IBookDomainService bookDomainService)
    : ICommandHandler<CreateBookCommand>
{
    public async Task Handler(CreateBookCommand command)
    {

        await validator.ValidateAndThrowAsync(command);
        
        var authorEntity = await authorRepository.GetById(command.AuthorId);

        if (authorEntity is null)
        {
            throw new AuthorNotFoundException(command.AuthorId);
        }
        
        if(!await bookDomainService.IsUniqueBookAsync(command.Title, command.ReleaseDate))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new ("", $"Author {authorEntity.ToModel().NameAndSurname} already has a book with the title {command.Title} and release date: {command.ReleaseDate.Date.ToShortDateString()}.")
            }); 
        }

        var newBookEntity = BookEntity.Create(command.Title, command.Description, command.ReleaseDate, command.AuthorId, command.Count, command.Price);
        
        authorEntity.AddBook();

        await authorRepository.UpdateAsync(authorEntity);
        await bookRepository.AddAsync(newBookEntity);

        await authorRepository.SaveAsync();
        await bookRepository.SaveAsync();
    }
}