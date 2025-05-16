

using System.Threading.Tasks;
using BookShop.Application.Commands;
using BookShop.Application.Commands.Handlers;
using BookShop.Domain;
using BookShop.Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class SoftDeleteAuthorCommandHandlerTest: TestBase
{

    private readonly SoftDeleteAuthorCommandHandler softDeleteAuthorCommandHandler;
    
    public SoftDeleteAuthorCommandHandlerTest()
    {
        softDeleteAuthorCommandHandler = Provider.GetService<SoftDeleteAuthorCommandHandler>();
    }

    [Fact]
    public async Task SoftDeleteAuthorCommandHandler_Success()
    {
        var author = CreateAuthor();
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
    
    
    private AuthorEntity CreateAuthor()
    {
        var author = AuthorEntity.Create("Test Name", "Test Surname");
        DbContext.Add(author);
        DbContext.SaveChanges();
        return author;
    }
}