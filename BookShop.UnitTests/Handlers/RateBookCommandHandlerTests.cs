using System;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Application.Commands;
using BookShop.Application.Commands.Handlers;
using BookShop.Domain.Entities.Rating;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;
using BookShop.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class RateBookCommandHandlerTests : TestBase
{
    
    private readonly RateBookCommandHandler rateBookCommandHandler;
    private readonly MockHelper mockHelper;
    private readonly IRatingRepository<BookRatingEntity> bookRatingRepository;
    
    public RateBookCommandHandlerTests()
    {
        rateBookCommandHandler = Provider.GetService<RateBookCommandHandler>();
        mockHelper = new MockHelper(DbContext);
        bookRatingRepository = Provider.GetService<IRatingRepository<BookRatingEntity>>();
    }
    
    [Fact]
    public async Task RateBook_ShouldReturnSuccess_WhenValidData()
    {
        // Arrange
        var user = await DbContext.Users.FirstOrDefaultAsync();
        var book = DbContext.Books.FirstOrDefault();
        var command = new RateBookCommand(book.Id, user.Id, 5);
        
        // Act
        await rateBookCommandHandler.Handler(command);
        
        // Assert
        var rating = await bookRatingRepository.GetByEntityAndUserIdsAsync(book.Id, user.Id);
        rating.ShouldNotBeNull();
        rating.Score.ShouldBe(command.Score);
    }
    
    [Fact]
    public async Task RateBook_ShouldUpdateRating_WhenAlreadyExists()
    {
        // Arrange
        var user = await DbContext.Users.FirstOrDefaultAsync();
        var book = DbContext.Books.FirstOrDefault();
        
        var initialCommand = new RateBookCommand(book.Id, user.Id, 3);
        await rateBookCommandHandler.Handler(initialCommand);
        
        var updateCommand = new RateBookCommand(book.Id, user.Id, 4);
        
        // Act
        await rateBookCommandHandler.Handler(updateCommand);
        
        // Assert
        var rating = await bookRatingRepository.GetByEntityAndUserIdsAsync(book.Id, user.Id);
        rating.ShouldNotBeNull();
        rating.Score.ShouldBe(updateCommand.Score);
    }
    
    [Fact]
    public async Task RateBook_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var book = DbContext.Books.FirstOrDefault();
        var command = new RateBookCommand(book.Id, Guid.Empty, 5); // Non-existent user ID
        
        // Act & Assert
        await Should.ThrowAsync<UserNotFoundException>(() => rateBookCommandHandler.Handler(command));
    }
    
    [Fact]
    public async Task RateBook_ShouldThrowException_WhenBookNotFound()
    {
        // Arrange
        var user = await DbContext.Users.FirstOrDefaultAsync();
        var command = new RateBookCommand(9999, user.Id, 5); // Non-existent book ID
        
        // Act & Assert
        await Should.ThrowAsync<BookNotFoundException>(() => rateBookCommandHandler.Handler(command));
    }

    [Fact]
    public async Task RateBook_Should_ThrowException_WhenRatingScoreIsMoreThan_5()
    {
        // Arrange
        var user = await DbContext.Users.FirstOrDefaultAsync();
        var book = DbContext.Books.FirstOrDefault();
        var command = new RateBookCommand(book.Id, user.Id, 6); // Invalid score
        
        // Act & Assert
        await Should.ThrowAsync<RateException>(() => rateBookCommandHandler.Handler(command));
    }
    
    [Fact]
    public async Task RateBook_Should_ThrowException_WhenRatingScoreIsLessThan_1()
    {
        // Arrange
        var user = await DbContext.Users.FirstOrDefaultAsync();
        var book = DbContext.Books.FirstOrDefault();
        var command = new RateBookCommand(book.Id, user.Id, 0); // Invalid score
        
        // Act & Assert
        await Should.ThrowAsync<RateException>(() => rateBookCommandHandler.Handler(command));
    }

}