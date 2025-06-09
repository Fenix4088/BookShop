using System.Security.Authentication;
using BookShop.Application.Abstractions;
using BookShop.Domain;
using BookShop.Domain.Entities.Rating;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Commands.Handlers;

public class SoftDeleteAuthorCommandHandler: ICommandHandler<SoftDeleteAuthorCommand>
{
    private readonly IAuthorRepository authorRepository;
    private readonly IBookRepository bookRepository;
    private readonly IRatingRepository<BookRatingEntity> bookRatingRepository;

    public SoftDeleteAuthorCommandHandler(
        IAuthorRepository authorRepository, 
        IBookRepository bookRepository,
        IRatingRepository<BookRatingEntity> bookRatingRepository
        )
    {
        this.authorRepository = authorRepository;
        this.bookRepository = bookRepository;
        this.bookRatingRepository = bookRatingRepository;
    }

    public async Task Handler(SoftDeleteAuthorCommand command)
    {
        var authorEntity = await authorRepository.GetById(command.AuthorId);

        if (authorEntity is null)
        {
            throw new AuthorNotFoundException(command.AuthorId);
        }

        authorRepository.SoftRemove(authorEntity);
        bookRepository.BulkSoftDelete(authorEntity.Id);
        await authorRepository.SaveAsync();
        await bookRepository.SaveAsync();
        await bookRatingRepository.SaveAsync();
    }
}