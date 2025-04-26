using BookShop.Application.Abstractions;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace BookShop.Application.Commands.Handlers;

public class UpdateAuthorCommandHandler: ICommandHandler<UpdateAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IValidator<UpdateAuthorCommand> _validator;
    
    public UpdateAuthorCommandHandler(IAuthorRepository authorRepository, IValidator<UpdateAuthorCommand> validator)
    {
        _authorRepository = authorRepository;
        _validator = validator;
    }

    public async Task Handler(UpdateAuthorCommand command)
    {
        await _validator.ValidateAndThrowAsync(command);
        
        if (!await _authorRepository.IsUniqueAuthorAsync(command.Name, command.Surname))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new ("", "An author with the same name and surname already exists.")
            });
        }
        
        var author = await _authorRepository.GetById(command.Id);

        if (author == null)
        {
            throw new AuthorNotFoundException(command.Id);
        }
        
        author.Update(command.Name, command.Surname);

        await _authorRepository.UpdateAsync(author);
        await _authorRepository.SaveAsync();
    }
}