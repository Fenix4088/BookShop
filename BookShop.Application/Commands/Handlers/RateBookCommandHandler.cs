using BookShop.Application.Abstractions;
using BookShop.Domain.Abstractions;
using BookShop.Domain.Entities.Rating;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Commands.Handlers;

public class RateBookCommandHandler : ICommandHandler<RateBookCommand>
{

    private readonly IRatingRepository<BookRatingEntity> bookRatingRepository;
    
    public RateBookCommandHandler(
        IRatingRepository<BookRatingEntity> bookRatingRepository)
    {
        this.bookRatingRepository = bookRatingRepository;
    }
    public async Task Handler(RateBookCommand command)
    {
        try
        {
            var bookRatingEntity = await bookRatingRepository.GetByIdAsync(command.BookId, command.UserId);

            if (bookRatingEntity is null)
            {
                var newBookingRatingEntity = BookRatingEntity.Create(command.BookId, command.UserId, command.Score);
                await bookRatingRepository.AddAsync(newBookingRatingEntity);
            }
            else
            {
                bookRatingEntity.Update(command.Score);
                await bookRatingRepository.UpdateAsync(bookRatingEntity);
            }
            
            await bookRatingRepository.SaveAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RateException("An error occurred while rating the author. Error: " + e.Message);
        }
        
    }
}