using System.Threading.Tasks;
using BookShop.Application.Queries;
using BookShop.Infrastructure.Handlers;
using BookShop.UnitTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class GetAuthorQueryHandlerTest: TestBase
{
    
    private readonly string validName = "Valid Name";
    private readonly string validSurname = "Valid Surname";
    private readonly GetAuthorQueryHandler getAuthorQueryHandler;
    private readonly MockHelper mockHelper;

    public GetAuthorQueryHandlerTest()
    {
        getAuthorQueryHandler = Provider.GetService<GetAuthorQueryHandler>();
        mockHelper = new MockHelper(DbContext);
    }

    [Fact]
    public async Task GetAuthorQueryHandler_Success()
    {
         var author = mockHelper.CreateAuthor();
         var query = new GetAuthorQuery(author.Id);

         var result = await getAuthorQueryHandler.Handler(query);

         Assert.NotNull(result);
         Assert.Equal(author.Id, result?.Id);
         Assert.Equal(author.Name, result?.Name);
         Assert.Equal(author.Surname, result?.Surname);
    }
    
    [Fact]
    public async Task GetAuthorQueryHandler_Should_Return_Null_When_Author_Not_Found()
    {
         var query = new GetAuthorQuery(0);

         var result = await getAuthorQueryHandler.Handler(query);

         Assert.Null(result);
    }
    
    // private AuthorEntity CreateAuthor()
    // {
    //      var author = AuthorEntity.Create(validName, validSurname);
    //      DbContext.Add(author);
    //      DbContext.SaveChanges();
    //      return author;
    // }
}