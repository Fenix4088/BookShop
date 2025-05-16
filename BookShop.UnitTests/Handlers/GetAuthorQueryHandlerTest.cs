using System.Threading.Tasks;
using BookShop.Application.Queries;
using BookShop.Domain;
using BookShop.Infrastructure.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class GetAuthorQueryHandlerTest: TestBase
{
    
    private readonly string validName = "Valid Name";
    private readonly string validSurname = "Valid Surname";
    private readonly GetAuthorQueryHandler getAuthorQueryHandler;

    public GetAuthorQueryHandlerTest()
    {
        getAuthorQueryHandler = Provider.GetService<GetAuthorQueryHandler>();
    }

    [Fact]
    public async Task GetAuthorQueryHandler_Success()
    {
         var author = CreateAuthor();
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
    
    private AuthorEntity CreateAuthor()
    {
         var author = AuthorEntity.Create(validName, validSurname);
         DbContext.Add(author);
         DbContext.SaveChanges();
         return author;
    }
}