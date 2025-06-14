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

public class SoftDeleteAuthorCommandHandlerTest: TestBase
{

    private readonly SoftDeleteAuthorCommandHandler softDeleteAuthorCommandHandler;
    private readonly MockHelper mockHelper;
    private readonly IRatingRepository<AuthorRatingEntity> authorRatingRepository;
    private readonly IRatingRepository<BookRatingEntity> bookRatingRepository;
    private readonly RateAuthorCommandHandler rateAuthorCommandHandler;
    private readonly RateBookCommandHandler rateBookCommandHandler;
    
    public SoftDeleteAuthorCommandHandlerTest()
    {
        authorRatingRepository = Provider.GetService<IRatingRepository<AuthorRatingEntity>>();
        bookRatingRepository = Provider.GetService<IRatingRepository<BookRatingEntity>>();
        
        softDeleteAuthorCommandHandler = Provider.GetService<SoftDeleteAuthorCommandHandler>();
        rateAuthorCommandHandler = Provider.GetService<RateAuthorCommandHandler>();
        rateBookCommandHandler = Provider.GetService<RateBookCommandHandler>();
        mockHelper = new MockHelper(DbContext);
    }

    [Fact]
    public async Task SoftDeleteAuthorCommandHandler_Success()
    {
        var author = mockHelper.CreateAuthor();
        var command = new SoftDeleteAuthorCommand(author.Id);

        await softDeleteAuthorCommandHandler.Handler(command);

        var deletedAuthor = await DbContext.Authors.FindAsync(author.Id);
        
        Assert.NotNull(deletedAuthor);
        Assert.True(deletedAuthor.IsDeleted);
    }
    
    [Fact]
    public async Task SoftDeleteAuthorCommandHandler_Should_Throw_Exception_When_Author_Not_Found()
    {
        var command = new SoftDeleteAuthorCommand(0);

        await Assert.ThrowsAsync<AuthorNotFoundException>(() => softDeleteAuthorCommandHandler.Handler(command));
    }
    
    [Fact]
    public async Task SoftDeleteAuthorCommandHandler_Should_Delete_Releted_Entities()
    {
        var author = mockHelper.CreateAuthor();
        var book = mockHelper.CreateBook(author);
        var user = await DbContext.Users.FirstOrDefaultAsync();

        //rate author before deletion
        var rateAuthorCommand = new RateAuthorCommand(author.Id, user.Id, 5);
        await rateAuthorCommandHandler.Handler(rateAuthorCommand);
        
        //rate book before deletion
        var rateBookCommand = new RateBookCommand(book.Id, user.Id, 4);
        await rateBookCommandHandler.Handler(rateBookCommand);

        var command = new SoftDeleteAuthorCommand(author.Id);
        await softDeleteAuthorCommandHandler.Handler(command);

        var deletedAuthor = await DbContext.Authors.FindAsync(author.Id);
        var deletedBook = await DbContext.Books.FindAsync(book.Id);
        var authorRatings = await authorRatingRepository.GetAllByEntityIdAsync(author.Id);
        var bookRatings = await bookRatingRepository.GetAllByEntityIdAsync(book.Id);
        

        deletedAuthor.ShouldNotBeNull();
        deletedAuthor.IsDeleted.ShouldBeTrue();
        deletedBook.ShouldNotBeNull();
        deletedBook.IsDeleted.ShouldBeTrue();
        authorRatings.Count.ShouldBe(1);
        authorRatings.First().DeletedAt.ShouldNotBeNull();
        bookRatings.Count.ShouldBe(1);
        bookRatings.First().DeletedAt.ShouldNotBeNull();
    }
}