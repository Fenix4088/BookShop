using BookShop.Application.Commands;
using BookShop.Domain;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Handlers.Abstractions;
using FluentValidation.Results;
using ValidationException = FluentValidation.ValidationException;

namespace BookShop.Application.Handlers.Commands;

public class CreateAuthorCommandHandler : ICommandHandler<CreateAuthorCommand>
{
    private readonly IRepository<AuthorEntity> _authorRepository;

    public CreateAuthorCommandHandler(IRepository<AuthorEntity> authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task Handler(CreateAuthorCommand command)
    {
        var failures = new List<ValidationFailure> { };
        // if (await _authorRepository.IsUniqueAuthorAsync(command))
        // {
        //     failures.Add(new ValidationFailure("", "An author with the same name and surname already exists."));
        // }

        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
        }

        var entity = AuthorEntity.Create(command.Name, command.Surname);
        await _authorRepository.Add(entity);
        await _authorRepository.Save();
    }
}