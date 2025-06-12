using BookShop.Application.Abstractions;
using BookShop.Domain;
using BookShop.Domain.Abstractions;
using BookShop.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using ValidationException = FluentValidation.ValidationException;

namespace BookShop.Application.Commands.Handlers;

public class CreateAuthorCommandHandler : ICommandHandler<CreateAuthorCommand>
{
    private readonly IAuthorRepository authorRepository;
    private readonly IValidator<CreateAuthorCommand> validator;
    private readonly IAuthorDomainService authorDomainService;

    public CreateAuthorCommandHandler(IAuthorRepository authorRepository,
        IValidator<CreateAuthorCommand> validator,
        IAuthorDomainService authorDomainService
        )
    {
        this.authorRepository = authorRepository;
        this.validator = validator;
        this.authorDomainService = authorDomainService;
    }

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