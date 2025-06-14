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

public class SoftDeleteBookCommandHandlerTest: TestBase
{
    
    private readonly SoftDeleteBookCommandHandler softDeleteBookCommandHandler;
    private readonly IRatingRepository<BookRatingEntity> bookRatingRepository;
    private readonly IBookRepository bookRepository;
    private readonly RateBookCommandHandler rateBookCommandHandler;
    
    private readonly MockHelper mockHelper;
    
    public SoftDeleteBookCommandHandlerTest()
    {
        softDeleteBookCommandHandler = Provider.GetService<SoftDeleteBookCommandHandler>(); ;
        bookRepository = Provider.GetService<IBookRepository>();
        bookRatingRepository = Provider.GetService<IRatingRepository<BookRatingEntity>>();
        rateBookCommandHandler = Provider.GetService<RateBookCommandHandler>();
        
        mockHelper = new MockHelper(DbContext);
    }

    [Fact]
    public async Task SoftDeleteBookCommandHandler_Success()
    {
        var author = mockHelper.CreateAuthor();
        var book = mockHelper.CreateBook(author);
        var command = new SoftDeleteBookCommand(book.Id);

        author.BookCount.ShouldBe(1);
        await softDeleteBookCommandHandler.Handler(command);

        var deletedBook = await bookRepository.GetBookById(book.Id);
        
        author.BookCount.ShouldBe(0);
        Assert.NotNull(deletedBook);
        Assert.True(deletedBook.IsDeleted);
    }
    
    [Fact]
    public async Task SoftDeleteBookCommandHandler_Should_Throw_Exception_When_Book_Not_Found()
    {
        var command = new SoftDeleteBookCommand(0);

        await Assert.ThrowsAsync<BookNotFoundException>(() => softDeleteBookCommandHandler.Handler(command));
    }
    
    [Fact]
    public async Task SoftDeleteBookCommandHandler_Should_Not_Decrease_AuthorBookCount_IfBookDosentExist()
    {
        var author = mockHelper.CreateAuthor();
        mockHelper.CreateBook(author);
        var command = new SoftDeleteBookCommand(9999);
        
        await Assert.ThrowsAsync<BookNotFoundException>(() => softDeleteBookCommandHandler.Handler(command));
        author.BookCount.ShouldBe(1);
    }
    
    [Fact]
    public async Task SoftDeleteBookCommandHandler_Should_Delete_Related_Entities() 
    {
        var author = mockHelper.CreateAuthor();
        var book = mockHelper.CreateBook(author);
        var user = await DbContext.Users.FirstOrDefaultAsync();
        
        // Rate book before deletion
        var rateBookCommand = new RateBookCommand(book.Id, user.Id, 4);
        await rateBookCommandHandler.Handler(rateBookCommand);
        
        
        
        var command = new SoftDeleteBookCommand(book.Id);
        await softDeleteBookCommandHandler.Handler(command);

        var deletedBook = await bookRepository.GetBookById(book.Id);
        var bookRatings = await bookRatingRepository.GetAllByEntityIdAsync(book.Id);
        
        
        deletedBook.ShouldNotBeNull();
        bookRatings.ShouldNotBeNull();
        bookRatings.Count.ShouldBe(1);
        bookRatings.First().DeletedAt.ShouldNotBeNull();
    }
    
    
}