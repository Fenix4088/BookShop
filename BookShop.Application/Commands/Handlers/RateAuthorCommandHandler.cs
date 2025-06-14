using BookShop.Application.Abstractions;
using BookShop.Application.Users;
using BookShop.Domain.Entities.Rating;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Commands.Handlers;

public class RateAuthorCommandHandler : ICommandHandler<RateAuthorCommand>
{
    private readonly IRatingRepository<AuthorRatingEntity> authorRatingRepository;
    private readonly IAuthorRepository authorRepository;
    private readonly IUserRepository userRepository;
    
    
    public RateAuthorCommandHandler(
        IRatingRepository<AuthorRatingEntity> authorRatingRepository,
        IAuthorRepository authorRepository,
        IUserRepository userRepository
        )
    {
        this.authorRatingRepository = authorRatingRepository;
        this.authorRepository = authorRepository;
        this.userRepository = userRepository;
    }
    
    public async Task Handler(RateAuthorCommand command)
    {
        var user = await userRepository.GetByIdAsync(command.UserId);
        
        if (user is null)
        {
            throw new UserNotFoundException(command.UserId);
        }
        
        var author = await authorRepository.GetById(command.AuthorId);
        
        if (author is null)
        {
            throw new AuthorNotFoundException(command.AuthorId);
        }
        
        
        var authorRatingEntity = await authorRatingRepository.GetByEntityAndUserIdsAsync(command.AuthorId, command.UserId);
        
        try
        {
            if (authorRatingEntity is null)
            {
                var newAuthorRatingEntity = AuthorRatingEntity.Create(command.AuthorId, command.UserId, command.Score);
                await authorRatingRepository.AddAsync(newAuthorRatingEntity);
            }
            else
            {
                authorRatingEntity.Update(command.Score);
                await authorRatingRepository.UpdateAsync(authorRatingEntity);
            }
            
            await authorRatingRepository.SaveAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RateException("An error occurred while rating the author. Error: " + e.Message);
        }
    }
}