using BookShop.Application.Abstractions;
using BookShop.Domain.Entities.Rating;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Commands.Handlers;

public class RateAuthorCommandHandler : ICommandHandler<RateAuthorCommand>
{
    private readonly IRatingRepository<AuthorRatingEntity> authorRatingRepository;
    
    public RateAuthorCommandHandler(
        IRatingRepository<AuthorRatingEntity> authorRatingRepository)
    {
        this.authorRatingRepository = authorRatingRepository;
    }
    
    public async Task Handler(RateAuthorCommand command)
    {
        
        var authorRatingEntity = await authorRatingRepository.GetByIdAsync(command.AuthorId, command.UserId);
        
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