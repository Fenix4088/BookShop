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
        InitTestData();
    }

    [Fact]
    public async Task GetAuthorListQueryHandler_Success()
    {
        var handler = Provider.GetService<GetAuthorListQueryHandler>();
        var query = mockHelper.CreateGetAuthorListQuery(
                pageSize: 4
            );

        var result = await handler.Handler(query);

        Assert.Equal(query.RowCount, result.PageSize);
        Assert.Equal(query.CurrentPage, result.CurrentPage);
        Assert.Equal(5, result.PageCount);
        Assert.Equal(20, result.TotalRowCount);
        Assert.NotEmpty(result.Items);
    }
    
    [Fact]
    public async Task GetAuthorListQueryHandler_Success_WithSearch()
    {
        var handler = Provider.GetService<GetAuthorListQueryHandler>();
        var query = mockHelper.CreateGetAuthorListQuery(
                searchByNameAndSurname: "Surname 1 Name 1"
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
        var query = mockHelper.CreateGetAuthorListQuery(
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
        var query = mockHelper.CreateGetAuthorListQuery(
                sortDirection: SortDirection.Ascending
            );

        var result = await handler.Handler(query);

        Assert.Equal(query.RowCount, result.PageSize);
        Assert.Equal(query.CurrentPage, result.CurrentPage);
        Assert.Equal(2, result.PageCount);
        Assert.Equal(20, result.TotalRowCount);
        Assert.NotEmpty(result.Items);
        
        var itemsList = result.Items.ToList();

        for (int i = 0; i < itemsList.Count - 1; i++)
        {
            Assert.True(string.Compare(itemsList[i].Name, itemsList[i + 1].Name) >= 0);
        }
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