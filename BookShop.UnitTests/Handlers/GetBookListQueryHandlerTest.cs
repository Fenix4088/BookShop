using System.Threading.Tasks;
using BookShop.Application.Commands;
using BookShop.Application.Commands.Handlers;
using BookShop.Application.Models;
using BookShop.Application.Queries.Handlers;
using BookShop.UnitTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class GetBookListQueryHandlerTest: TestBase
{
    private readonly MockHelper mockHelper;
    private readonly GetBookListQueryHandler getBookListQueryHandler;
    private readonly SoftDeleteBookCommandHandler softDeleteBookCommandHandler;
    
    public GetBookListQueryHandlerTest()
    {
        mockHelper = new MockHelper(DbContext);
        getBookListQueryHandler = Provider.GetService<GetBookListQueryHandler>();
        softDeleteBookCommandHandler = Provider.GetService<SoftDeleteBookCommandHandler>();
    }
    
    [Fact]
    public async Task GetBookList_ShouldReturnPagedResult_WhenBooksExist()
    {
        // Arrange
        mockHelper.CreateAuthorWithBooks();
        var query = mockHelper.CreateGetBookListQuery();
        
        // Act
        var result = await getBookListQueryHandler.Handler(query);
        
        // Assert
        result.ShouldNotBeNull();
        result.Items.ShouldNotBeEmpty();
        result.Items.ShouldAllBe(x => x.GetType() == typeof(BookModel));
        result.TotalRowCount.ShouldBeGreaterThan(0);
        result.PageSize.ShouldBe(10);
        result.PageCount.ShouldBe(1);
    }

    [Fact]
    public async Task GetBookList_ShouldNotReturnBooks_WhenBookWasDelete()
    {
        // Arrange
        var author = mockHelper.CreateAuthor();
        var book = mockHelper.CreateBook(author);
        DbContext.SaveChanges();
        
        
        var query = mockHelper.CreateGetBookListQuery();
        
        // Act
        await softDeleteBookCommandHandler.Handler(new SoftDeleteBookCommand(book.Id));
        var result = await getBookListQueryHandler.Handler(query);
        
        // Assert
        result.ShouldNotBeNull();
        result.Items.ShouldBeEmpty();
        result.TotalRowCount.ShouldBe(0);
    }
}