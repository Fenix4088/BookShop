using System;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Application.Commands;
using BookShop.Application.Commands.Handlers;
using BookShop.Application.Queries;
using BookShop.Domain.Exceptions;
using BookShop.Infrastructure.Handlers;
using BookShop.UnitTests.Helpers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class CreateBookCommandHandlerTest: TestBase
{
    private readonly MockHelper mockHelper;
    private readonly CreateBookCommandHandler createBookCommandHandler;
    private readonly GetBookListQueryHandler getBookListQueryHandler;
    
    public CreateBookCommandHandlerTest()
    {
        mockHelper = new MockHelper(DbContext);
        createBookCommandHandler = Provider.GetService<CreateBookCommandHandler>();
        getBookListQueryHandler = Provider.GetService<GetBookListQueryHandler>();
    }
    
    [Fact]
    public async Task CreateBook_ShouldReturnCreatedBook_WhenValidData()
    {
        // Arrange
        var author = mockHelper.CreateAuthor();
        var command = new CreateBookCommand(author.Id, "Test Book", "Test Description", DateTime.Now.AddDays(-1));
        
        // Act
        await createBookCommandHandler.Handler(command);
        var result = (await getBookListQueryHandler.Handler(new GetBookListQuery(1, 10))).Items.FirstOrDefault();
        
        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Title.ShouldBe(command.Title);
        result.Description.ShouldBe(command.Description);
        result.AuthorId.ShouldBe(command.AuthorId);
    }
    
    [Fact]
    public async Task CreateBook_ShouldThrowException_WhenAuthorDoesNotExist()
    {
        // Arrange
        var command = new CreateBookCommand(9999, "Test Book", "Test Description", DateTime.Now.AddDays(-1));
        
        // Act & Assert
        await Should.ThrowAsync<AuthorNotFoundException>(async () => await createBookCommandHandler.Handler(command));
    }
    
    [Fact]
    public async Task CreateBook_ShouldThrowException_WhenBookWithTheSameTitleAndReleaseDate_AlreadyExist()
    {
        // Arrange
        var author = mockHelper.CreateAuthor();
        var command = new CreateBookCommand(author.Id, "Test Book", "Test Description", DateTime.Now.AddDays(-1));
        
        // Act
        await createBookCommandHandler.Handler(command);
        
        // Assert
        var exception = await Should.ThrowAsync<ValidationException>(async () => await createBookCommandHandler.Handler(command));
        
        exception.Message.ShouldContain("already has a book with the title");
    }
    
    [Fact]
    public async Task CreateBook_Should_IncreaseAuthorBookCount_WhenBookIsCreated()
    {
        // Arrange
        var author = mockHelper.CreateAuthor();
        var command = new CreateBookCommand(author.Id, "Test Book", "Test Description", DateTime.Now.AddDays(-1));
        
        // Act
        await createBookCommandHandler.Handler(command);
        
        // Assert
        author.BookCount.ShouldBe(1);
    }
    
}