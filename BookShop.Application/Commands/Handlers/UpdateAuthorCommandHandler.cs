using BookShop.Application.Abstractions;
using BookShop.Domain.Abstractions;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace BookShop.Application.Commands.Handlers;

public class UpdateAuthorCommandHandler(
    IAuthorRepository authorRepository,
    IValidator<UpdateAuthorCommand> validator,
    IAuthorDomainService authorDomainService)
    : ICommandHandler<UpdateAuthorCommand>
{
    public async Task Handler(UpdateAuthorCommand command)
    {
        await validator.ValidateAndThrowAsync(command);
        
        if (!await authorDomainService.IsUniqueAuthorAsync(command.Name, command.Surname))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new ("", "An author with the same name and surname already exists.")
            });
        }
        
        var author = await authorRepository.GetById(command.Id);

        if (author == null)
        {
            throw new AuthorNotFoundException(command.Id);
        }
        
        author.Update(command.Name, command.Surname);

        await authorRepository.UpdateAsync(author);
        await authorRepository.SaveAsync();
    }
}