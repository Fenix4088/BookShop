using System.Threading.Tasks;
using BookShop.Domain;
using BookShop.Infrastructure.Handlers;
using BookShop.Models.Queries;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookShop.UnitTests.Handlers;

public class GetAuthorListQueryHandlerTests: TestBase
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        InitTestData();
    }

    [Fact]
    public async Task GetAuthorListQueryHandler_Success()
    {
        var handle = Provider.GetService<GetAuthorListQueryHandler>();
        var query = new GetAuthorListQuery(1, 5);

        var result = await handle.Handler(query);

        Assert.Equal(query.RowCount, result.PageSize);
        Assert.Equal(query.CurrentPage, result.CurrentPage);
        Assert.Equal(4, result.PageCount);
        Assert.Equal(20, result.TotalRowCount);
        Assert.NotEmpty(result.Items);
    }

    private void InitTestData()
    {
        for (int i = 0; i < 20; i++)
        {
            DbContext.Add(AuthorEntity.Create($"Name {i}", $"Surame {i}"));
        }
        DbContext.SaveChanges();
    }
}