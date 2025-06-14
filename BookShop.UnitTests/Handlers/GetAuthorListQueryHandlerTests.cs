using System.Linq;
using System.Threading.Tasks;
using BookShop.Application.Queries.Handlers;
using BookShop.Domain;
using BookShop.Shared.Enums;
using BookShop.UnitTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class GetAuthorListQueryHandlerTests: TestBase
{
    
    private readonly MockHelper mockHelper;
    
    public GetAuthorListQueryHandlerTests()
    {
        mockHelper = new MockHelper(DbContext);
    }
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        // InitTestData();
    }

    [Fact]
    public async Task GetAuthorListQueryHandler_Success()
    {
        var handler = Provider.GetService<GetAuthorListQueryHandler>();
        var query = mockHelper.GenerateGetAuthorListQuery(
                pageSize: 4
            );

        var result = await handler.Handler(query);

        Assert.Equal(query.RowCount, result.PageSize);
        Assert.Equal(query.CurrentPage, result.CurrentPage);
        Assert.Equal(3, result.PageCount);
        Assert.Equal(10, result.TotalRowCount);
        Assert.NotEmpty(result.Items);
    }
    
    [Fact]
    public async Task GetAuthorListQueryHandler_Success_WithSearch()
    {
        var author = mockHelper.CreateAuthor();
        var handler = Provider.GetService<GetAuthorListQueryHandler>();
        var query = mockHelper.GenerateGetAuthorListQuery(
                searchByNameAndSurname: author.Surname + " " + author.Name
            );

        var result = await handler.Handler(query);

        Assert.Equal(query.RowCount, result.PageSize);
        Assert.Equal(query.CurrentPage, result.CurrentPage);
        Assert.Equal(1, result.PageCount);
        Assert.Equal(1, result.TotalRowCount);
        Assert.NotEmpty(result.Items);
    }
    
    [Fact]
    public async Task GetAuthorListQueryHandler_Should_Return_EmptyList_If_Provided_With_Invalid_Search()
    {
        var handler = Provider.GetService<GetAuthorListQueryHandler>();
        var query = mockHelper.GenerateGetAuthorListQuery(
                searchByNameAndSurname: "Invalid Name"
            );

        var result = await handler.Handler(query);

        Assert.Equal(query.RowCount, result.PageSize);
        Assert.Equal(query.CurrentPage, result.CurrentPage);
        Assert.Equal(0, result.PageCount);
        Assert.Equal(0, result.TotalRowCount);
        Assert.Empty(result.Items);
    }
    
    [Fact]
    public async Task GetAuthorListQueryHandler_Should_Sort_By_Name_Asc()
    {
        var handler = Provider.GetService<GetAuthorListQueryHandler>();
        var query = mockHelper.GenerateGetAuthorListQuery(
                sortDirection: SortDirection.Ascending,
                pageSize: 1000                
            );

        var result = await handler.Handler(query);

        Assert.Equal(query.RowCount, result.PageSize);
        Assert.Equal(query.CurrentPage, result.CurrentPage);
        Assert.Equal(1, result.PageCount);
        Assert.Equal(10, result.TotalRowCount);
        Assert.NotEmpty(result.Items);
        
        var itemsList = result.Items.ToList();
        var expectedList = itemsList.OrderBy(x => x.Surname).ToList();

        
        Assert.True(expectedList.SequenceEqual(itemsList));
    }

    private void InitTestData()
    {
        for (int i = 0; i < 20; i++)
        {
            DbContext.Add(AuthorEntity.Create($"Name {i}", $"Surname {i}"));
        }
        DbContext.SaveChanges();
    }
}