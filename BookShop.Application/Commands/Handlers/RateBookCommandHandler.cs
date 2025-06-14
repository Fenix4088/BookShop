using BookShop.Application.Abstractions;
using BookShop.Application.Users;
using BookShop.Domain.Entities.Rating;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Commands.Handlers;

public class RateBookCommandHandler : ICommandHandler<RateBookCommand>
{

    private readonly IRatingRepository<BookRatingEntity> bookRatingRepository;
    private readonly IUserRepository userRepository;
    private readonly IBookRepository bookRepository;
    
    public RateBookCommandHandler(
        IRatingRepository<BookRatingEntity> bookRatingRepository,
        IUserRepository userRepository,
        IBookRepository bookRepository
        )
    {
        this.bookRatingRepository = bookRatingRepository;
        this.userRepository = userRepository;
        this.bookRepository = bookRepository;
    }
    public async Task Handler(RateBookCommand command)
    {
        
        var user = await userRepository.GetByIdAsync(command.UserId);
        
        if (user is null)
        {
            throw new UserNotFoundException(command.UserId);
        }
        
        
        var book = await bookRepository.GetBookById(command.BookId);
        
        if (book is null)
        {
            throw new BookNotFoundException(command.BookId);
        }
        
        try
        {
            var bookRatingEntity = await bookRatingRepository.GetByEntityAndUserIdsAsync(command.BookId, command.UserId);

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