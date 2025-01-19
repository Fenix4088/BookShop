using BookShop.Application.Abstractions;
using BookShop.Domain;
using BookShop.Domain.Repositories;
using FluentValidation.Results;
using ValidationException = FluentValidation.ValidationException;

namespace BookShop.Application.Commands.Handlers;

public class CreateAuthorCommandHandler : ICommandHandler<CreateAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository;

    public CreateAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task Handler(CreateAuthorCommand command)
    {
        
        if (!await _authorRepository.IsUniqueAuthorAsync(command.Name, command.Surname))
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new ("", "An author with the same name and surname already exists.")
            });
        }
        

        var entity = AuthorEntity.Create(command.Name, command.Surname);
        await _authorRepository.Add(entity);
        await _authorRepository.Save();
    }
}