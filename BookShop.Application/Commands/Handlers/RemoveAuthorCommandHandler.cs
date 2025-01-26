using BookShop.Application.Abstractions;
using BookShop.Domain;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Commands.Handlers;

public class RemoveAuthorCommandHandler: ICommandHandler<RemoveAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository;

    public RemoveAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task Handler(RemoveAuthorCommand command)
    {
        var authorEntity = await _authorRepository.GetById(command.AuthorId);

        if (authorEntity is null)
        {
            //TODO: refactor into custom error
            throw new Exception("author not found");
        }

        _authorRepository.Remove(authorEntity);
        await _authorRepository.SaveAsync();
    }
}