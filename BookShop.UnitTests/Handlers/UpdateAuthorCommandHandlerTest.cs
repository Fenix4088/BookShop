using System.Threading.Tasks;
using BookShop.Application.Commands;
using BookShop.Application.Commands.Handlers;
using BookShop.Domain.Exceptions;
using BookShop.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class UpdateAuthorCommandHandlerTest: TestBase
{
    
    private readonly UpdateAuthorCommandHandler updateAuthorCommandHandler;

    private readonly string validName = "Billy";
    private readonly string validSurname = "Milligan";
    private readonly string newName = "NewName";
    private readonly string newSurname = "NewSurname";
    private readonly MockHelper mockHelper;

    public UpdateAuthorCommandHandlerTest()
    {
        updateAuthorCommandHandler = Provider.GetService<UpdateAuthorCommandHandler>();
        mockHelper = new MockHelper(DbContext);
    }

    [Fact]
    public async Task UpdateAuthorCommandHandler_Success()
    {
        var author = mockHelper.CreateAuthor();

        var command = new UpdateAuthorCommand(author.Id, newName, newSurname);

        await updateAuthorCommandHandler.Handler(command);

        var entity = await DbContext.Authors.FirstOrDefaultAsync();
        Assert.NotNull(entity);
        Assert.Equal(command.Name, entity.Name);
        Assert.Equal(command.Surname, entity.Surname);
    }
    
    [Fact]
    public async Task UpdateAuthorCommandHandler_ValidationException()
    {
        var author = mockHelper.CreateAuthor();

        var command = new UpdateAuthorCommand(author.Id, author.Name, author.Surname);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
        {
            await updateAuthorCommandHandler.Handler(command);
        });
    }
    
    [Fact]
    public async Task UpdateAuthorCommandHandler_ValidationException_EmptyName()
    {
        var author = mockHelper.CreateAuthor();

        var command = new UpdateAuthorCommand(author.Id, "", validSurname);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
        {
            await updateAuthorCommandHandler.Handler(command);
        });
    }
    
    [Fact]
    public async Task UpdateAuthorCommandHandler_ValidationException_EmptySurname()
    {
        var author = mockHelper.CreateAuthor();

        var command = new UpdateAuthorCommand(author.Id, validName, "");

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
        {
            await updateAuthorCommandHandler.Handler(command);
        });
    }
    
    [Fact]
    public async Task UpdateAuthorCommandHandler_ValidationException_If_Name_Already_Exists()
    {
        var author1 = mockHelper.CreateAuthor();
        var author2 = mockHelper.CreateAuthor();

        var command = new UpdateAuthorCommand(author2.Id, author1.Name, author1.Surname);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
        {
            await updateAuthorCommandHandler.Handler(command);
        });
    }
    
    [Fact]
    public async Task UpdateAuthorCommandHandler_ValidationException_If_Author_Not_Found()
    {
        var command = new UpdateAuthorCommand(100, newName, newSurname);

        await Assert.ThrowsAsync<AuthorNotFoundException>(async () =>
        {
            await updateAuthorCommandHandler.Handler(command);
        });
    }
}