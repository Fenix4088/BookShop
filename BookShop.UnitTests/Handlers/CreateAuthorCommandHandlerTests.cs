using System.Threading.Tasks;
using BookShop.Application.Commands;
using BookShop.Application.Commands.Handlers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class CreateAuthorCommandHandlerTests: TestBase
{
    const string InvalidName = "InvalidNameInvalidNameInvalidNameInvalidNameInvalidNameInvalidNameInvalidNameInvalidNameInvalidNameInvalidNameInvalidNameInvalidName";
    const string InvalidSurname = "InvalidSurnameInvalidSurnameInvalidSurnameInvalidSurnameInvalidSurnameInvalidSurnameInvalidSurnameInvalidSurnameInvalidSurnameInvalidSurnameInvalidSurnameInvalidSurname";
    
    private readonly CreateAuthorCommandHandler handler;

    private readonly string validName = "Billy";
    private readonly string validSurname = "Milligan";

    public CreateAuthorCommandHandlerTests()
    {
        handler = Provider.GetService<CreateAuthorCommandHandler>();
    }

    [Fact]
    public async Task CreateAuthorCommandHandler_Success()
    {
        var command = new CreateAuthorCommand(validName, validSurname);

        await handler.Handler(command);

        var entity = await DbContext.Authors.FirstOrDefaultAsync(x => x.Name == command.Name && x.Surname == command.Surname);
        Assert.NotNull(entity);
        Assert.False(entity.IsDeleted);
        Assert.NotEqual(0, entity.Id);
        Assert.Equal(command.Name, entity.Name);
        Assert.Equal(command.Surname, entity.Surname);
        Assert.Equal(0, entity.BookCount);
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData(InvalidName, InvalidSurname)]
    public async Task CreateAuthorCommandHandler_WithInvalidNames_ShouldThrow_ValidationError(string name, string surname)
    {
        var command = new CreateAuthorCommand(name, surname);

        await Assert.ThrowsAsync<ValidationException>(async () => await handler.Handler(command));
        
        var entity = await DbContext.Authors.FirstOrDefaultAsync(x => x.Name == command.Name && x.Surname == command.Surname);
        Assert.Null(entity);
    }
    
    [Fact]
    public async Task CreateAuthorCommandHandler_NameAndSurname_ShouldBe_Unique()
    {
        var commandBillyOne = new CreateAuthorCommand(validName, validSurname);
        var commandBillyTwo = new CreateAuthorCommand(validName, validSurname);

        await handler.Handler(commandBillyOne);
        
        var exception = await Assert.ThrowsAsync<ValidationException>(async () => await handler.Handler(commandBillyTwo));
        
        Assert.Contains("An author with the same name and surname already exists.", exception.Message);
    }
    
}

