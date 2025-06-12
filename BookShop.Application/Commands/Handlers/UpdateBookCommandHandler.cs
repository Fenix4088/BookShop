using BookShop.Application.Abstractions;
using BookShop.Domain.Abstractions;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using ValidationException = FluentValidation.ValidationException;

namespace BookShop.Application.Commands.Handlers;

public class UpdateBookCommandHandler: ICommandHandler<UpdateBookCommand>
{
    private readonly IBookRepository bookRepository;
    private readonly IAuthorRepository authorRepository;
    private readonly IValidator<UpdateBookCommand> validator;
    private readonly IBookDomainService bookDomainService;
    

    public UpdateBookCommandHandler(IBookRepository bookRepository, 
        IAuthorRepository authorRepository, 
        IValidator<UpdateBookCommand> validator,
        IBookDomainService bookDomainService
        )
    {
        this.bookRepository = bookRepository;
        this.authorRepository = authorRepository;
        this.validator = validator;
        this.bookDomainService = bookDomainService;
    }

    public async Task Handler(UpdateBookCommand command)
    {
        await validator.ValidateAndThrowAsync(command);
        
        var bookEntity = await bookRepository.GetBookById(command.Id);

        if (bookEntity is null) throw new BookNotFoundException(command.Id);

        var authorEntity = await authorRepository.GetById(command.AuthorId);

        if (authorEntity is null) throw new AuthorNotFoundException(command.AuthorId);
        
        if(!await bookDomainService.IsUniqueBookAsync(command.Title, command.ReleaseDate))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new ("", $"Author {authorEntity.ToModel().NameAndSurname} already has a book with the title {command.Title} and release date: {command.ReleaseDate.Date.ToShortDateString()}.")
            }); 
        }
        
        bookEntity.Update(authorEntity, command.Title, command.Description, command.ReleaseDate);
        await bookRepository.SaveAsync();
        await authorRepository.SaveAsync();

    }
}