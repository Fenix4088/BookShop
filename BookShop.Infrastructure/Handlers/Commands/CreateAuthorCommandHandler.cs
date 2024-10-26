using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookShop.Domain;
using BookShop.Infrastructure.Handlers.Abstractions;
using BookShop.Infrastructure.Repositories;
using BookShop.Models.Commands;
using System.Threading.Tasks;
using FluentValidation.Results;
using ValidationException = FluentValidation.ValidationException;

namespace BookShop.Infrastructure.Handlers.Commands;

public class CreateAuthorCommandHandler : ICommandHandler<CreateAuthorCommand>
{
    private readonly AuthorRepository _authorRepository;

    public CreateAuthorCommandHandler(AuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task Handler(CreateAuthorCommand command)
    {
        var failures = new List<ValidationFailure> { };
        if (await _authorRepository.IsUniqueAuthorAsync(command))
        {
            failures.Add(new ValidationFailure("", "An author with the same name and surname already exists."));
        }

        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
        }

        var entity = AuthorEntity.Create(command.Name, command.Surname);
        await _authorRepository.Add(entity);
        await _authorRepository.Save();
    }
}