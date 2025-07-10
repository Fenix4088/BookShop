using BookShop.Application.Abstractions;
using BookShop.Domain;
using BookShop.Domain.Abstractions;
using BookShop.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace BookShop.Application.Commands.Handlers;

public class CreateAuthorCommandHandler(
    IAuthorRepository authorRepository,
    IValidator<CreateAuthorCommand> validator,
    IAuthorDomainService authorDomainService)
    : ICommandHandler<CreateAuthorCommand>
{
    public async Task Handler(CreateAuthorCommand command)
    {
        await validator.ValidateAndThrowAsync(command);
        
        if (!await authorDomainService.IsUniqueAuthorAsync(command.Name, command.Surname))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new ("", "An author with the same name and surname already exists.")
            });
        }
        

        var entity = AuthorEntity.Create(command.Name, command.Surname);
        await authorRepository.AddAsync(entity);
        await authorRepository.SaveAsync();
    }
}