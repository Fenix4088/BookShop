using System.Threading.Tasks;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Application.Queries.Handlers;
using BookShop.Domain.Exceptions;
using BookShop.UnitTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class GetBookQueryHandlerTest: TestBase
{
    private readonly MockHelper mockHelper;
    private readonly GetBookQueryHandler getBookQueryHandler;
    
    public GetBookQueryHandlerTest()
    {
        mockHelper = new MockHelper(DbContext);
        getBookQueryHandler = Provider.GetService<GetBookQueryHandler>();
    }
    
    [Fact]
    public async Task GetBookById_ShouldReturnBook_WhenBookExists()
    {
        // Arrange
        var author = mockHelper.CreateAuthor();
        var book = mockHelper.CreateBook(author);
        var query = new GetBookQuery(book.Id);
        
        
        var result = await getBookQueryHandler.Handler(query);
        
        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(book.Id);
        result.Title.ShouldBe(book.Title);
        result.Description.ShouldBe(book.Description);
        result.AuthorId.ShouldBe(book.AuthorId);
        result.ShouldBeOfType<BookModel>();
    }
    
    [Fact]
    public async Task GetBookById_ShouldThrowException_WhenBookDoesNotExist()
    {
        // Arrange
        var query = new GetBookQuery(9999);
        
        // Act & Assert
        await Should.ThrowAsync<BookNotFoundException>(async () => await getBookQueryHandler.Handler(query));
    }
}