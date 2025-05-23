using System;
using System.Threading.Tasks;
using BookShop.Application.Commands;
using BookShop.Application.Commands.Handlers;
using BookShop.Infrastructure.Handlers;
using BookShop.Infrastructure.Repositories;
using BookShop.UnitTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class UpdateBookCommandHandlerTest: TestBase
{
    
    private readonly UpdateBookCommandHandler updateBookCommandHandler;
    private readonly GetBookListQueryHandler getBookListQueryHandler;
    private readonly BookRepository bookRepository;
    private readonly MockHelper mockHelper;
    
    public UpdateBookCommandHandlerTest()
    {
        updateBookCommandHandler = Provider.GetService<UpdateBookCommandHandler>();
        getBookListQueryHandler = Provider.GetService<GetBookListQueryHandler>();
        mockHelper = new MockHelper(DbContext);
        bookRepository = new BookRepository(DbContext);
    }
    
    [Fact]
    public async Task UpdateBookCommandHandler_ShouldUpdateBook_WhenValidCommand()
    {
        // Arrange
        var author = mockHelper.CreateAuthor();
        var book = mockHelper.CreateBook(author);
        
        
        // Act
        var command = new UpdateBookCommand(book.Id, book.AuthorId, "Updated Title", "Updated Description", DateTime.Now.AddDays(-1));
        
        await updateBookCommandHandler.Handler(command);
        
        var updatedBook = await bookRepository.GetBookById(book.Id);
        
        // Assert
        updatedBook.ShouldNotBeNull();
        updatedBook.Title.ShouldBe(command.Title);
        updatedBook.Description.ShouldBe(command.Description);
        updatedBook.ReleaseDate.ShouldBe(command.ReleaseDate);
    }
    
    [Fact]
    public async Task UpdateBookCommandHandler_Should_Not_Change_AuthorBookCount_WhenAuthorId_Not_Changed()
    {
        // Arrange
        var author = mockHelper.CreateAuthor();
        var book = mockHelper.CreateBook(author);
        
        author.BookCount.ShouldBe(1);

        // Act
        var command = new UpdateBookCommand(book.Id, book.AuthorId, "Updated Title", "Updated Description", DateTime.Now.AddDays(-1));
        
        await updateBookCommandHandler.Handler(command);
        
        var updatedBook = await bookRepository.GetBookById(book.Id);
        
        // Assert
        updatedBook.ShouldNotBeNull();
        updatedBook.Title.ShouldBe(command.Title);
        updatedBook.Description.ShouldBe(command.Description);
        updatedBook.ReleaseDate.ShouldBe(command.ReleaseDate);
        author.BookCount.ShouldBe(1);
    }

    [Fact]
    public async Task UpdateBookCommandHandler_Should_Change_AuthorBookCount_For_NewAndOldAuthors_If_BookAuthorIdWasChanged()
    {
        var oldAuthor = mockHelper.CreateAuthor();
        var newAuthor = mockHelper.CreateAuthor();
        var book = mockHelper.CreateBook(oldAuthor);
        
        oldAuthor.BookCount.ShouldBe(1);
        newAuthor.BookCount.ShouldBe(0);
        
        // Act
        var command = new UpdateBookCommand(book.Id, newAuthor.Id, "Updated Title", "Updated Description", DateTime.Now.AddDays(-1));
        await updateBookCommandHandler.Handler(command);
        
        oldAuthor.BookCount.ShouldBe(0);
        newAuthor.BookCount.ShouldBe(1);
        
    }
}