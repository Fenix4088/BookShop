using System;
using System.Threading.Tasks;
using BookShop.Application.Commands;
using BookShop.Application.Commands.Handlers;
using BookShop.Domain.Entities.Rating;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;
using BookShop.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class RateAuthorCommandHandlerTests : TestBase
{
    private readonly RateAuthorCommandHandler rateAuthorCommandHandler;
    private readonly MockHelper mockHelper;
    private readonly IRatingRepository<AuthorRatingEntity> authorRatingRepository;
    
    
    public RateAuthorCommandHandlerTests()
    {
        rateAuthorCommandHandler = Provider.GetService<RateAuthorCommandHandler>();
        mockHelper = new MockHelper(DbContext);
        authorRatingRepository = Provider.GetService<IRatingRepository<AuthorRatingEntity>>();
    }
    
    [Fact]
    public async Task RateAuthor_ShouldReturnSuccess_WhenValidData()
    {
        // Arrange
        var author = mockHelper.CreateAuthor();

        var user = await DbContext.Users.FirstOrDefaultAsync();
        
        var command = new RateAuthorCommand(author.Id, user.Id, 5);
        // Act
        await rateAuthorCommandHandler.Handler(command);
        
        // Assert
        var rating = await authorRatingRepository.GetByEntityAndUserIdsAsync(author.Id, user.Id);
        Assert.NotNull(rating);
        Assert.Equal(command.Score, rating.Score);
    }

    [Fact]
    public async Task RateAuthor_ShouldUpdateRating_WhenAlreadyExists()
    {
        // Arrange
        var author = mockHelper.CreateAuthor();
        var user = await DbContext.Users.FirstOrDefaultAsync();
        
        var initialCommand = new RateAuthorCommand(author.Id, user.Id, 3);
        await rateAuthorCommandHandler.Handler(initialCommand);
        
        var updateCommand = new RateAuthorCommand(author.Id, user.Id, 4);
        
        // Act
        await rateAuthorCommandHandler.Handler(updateCommand);
        
        // Assert
        var rating = await authorRatingRepository.GetByEntityAndUserIdsAsync(author.Id, user.Id);
        Assert.NotNull(rating);
        Assert.Equal(updateCommand.Score, rating.Score);
    }
    
    [Fact]
    public async Task RateAuthor_ShouldThrowException_WhenAuthorNotFound()
    {
        // Arrange
        var user = await DbContext.Users.FirstOrDefaultAsync();
        var command = new RateAuthorCommand(9999, user.Id, 5); // Non-existent author ID
        
        // Act & Assert
        await Assert.ThrowsAsync<AuthorNotFoundException>(() => rateAuthorCommandHandler.Handler(command));
    }
    
    [Fact]
    public async Task RateAuthor_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var author = mockHelper.CreateAuthor();
        var command = new RateAuthorCommand(author.Id, Guid.NewGuid(), 5); // Non-existent user ID
        
        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => rateAuthorCommandHandler.Handler(command));
    }

    [Fact]
    public async Task RateAuthor_ShouldThrow_AnException_If_Rating_Is_Bigger_Than_5()
    {
        // Arrange
        var author = mockHelper.CreateAuthor();
        var user = await DbContext.Users.FirstOrDefaultAsync();
        
        var command = new RateAuthorCommand(author.Id, user.Id, 6); // Invalid score
        
        // Act & Assert
        await Assert.ThrowsAsync<RateException>(() => rateAuthorCommandHandler.Handler(command));
    }
    
    [Fact]
    public async Task RateAuthor_ShouldThrow_AnException_If_Rating_Is_Less_Than_1()
    {
        // Arrange
        var author = mockHelper.CreateAuthor();
        var user = await DbContext.Users.FirstOrDefaultAsync();
        
        var command = new RateAuthorCommand(author.Id, user.Id, 0); // Invalid score
        
        // Act & Assert
        await Assert.ThrowsAsync<RateException>(() => rateAuthorCommandHandler.Handler(command));
    }
}