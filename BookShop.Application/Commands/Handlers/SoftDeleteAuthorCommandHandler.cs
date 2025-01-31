using System.Security.Authentication;
using BookShop.Application.Abstractions;
using BookShop.Domain;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Commands.Handlers;

public class SoftDeleteAuthorCommandHandler: ICommandHandler<SoftDeleteAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository;

    public SoftDeleteAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task Handler(SoftDeleteAuthorCommand command)
    {
        var authorEntity = await _authorRepository.GetById(command.AuthorId);

        if (authorEntity is null)
        {
            throw new AuthorNotFoundException(command.AuthorId);
        }

        _authorRepository.SoftRemove(authorEntity);
        await _authorRepository.SaveAsync();
    }
}