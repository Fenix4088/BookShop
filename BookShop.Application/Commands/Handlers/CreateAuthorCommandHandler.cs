using BookShop.Application.Abstractions;
using BookShop.Application.Enums;
using BookShop.Domain;
using BookShop.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using ValidationException = FluentValidation.ValidationException;

namespace BookShop.Application.Commands.Handlers;

public class CreateAuthorCommandHandler : ICommandHandler<CreateAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IValidator<CreateAuthorCommand> _validator;

    public CreateAuthorCommandHandler(IAuthorRepository authorRepository, IValidator<CreateAuthorCommand> validator)
    {
        _authorRepository = authorRepository;
        _validator = validator;
    }

    public async Task Handler(CreateAuthorCommand command)
    {
        await _validator.ValidateAndThrowAsync(command);
        
        if (!await _authorRepository.IsUniqueAuthorAsync(command.Name, command.Surname))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new ("", "An author with the same name and surname already exists.")
            });
        }
        

        var entity = AuthorEntity.Create(command.Name, command.Surname);
        await _authorRepository.AddAsync(entity);
        await _authorRepository.SaveAsync();
    }
}