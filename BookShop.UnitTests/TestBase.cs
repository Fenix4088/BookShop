using System;
using System.Threading;
using System.Threading.Tasks;
using BookShop.Infrastructure.Abstractions;
using BookShop.Infrastructure.Context;
using BookShop.UnitTests.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookShop.UnitTests;

public abstract class TestBase: IAsyncLifetime, IDisposable
{
    private readonly IServiceProvider _provider;

    protected ShopDbContext DbContext { get; }
    protected IServiceProvider Provider => _provider;

    protected TestBase()
    {
        var services = new ServiceCollection()
            .AddBookShopTestDeps();

        _provider = services.BuildServiceProvider();
        DbContext = _provider.GetService<ShopDbContext>();
        
    }


    public virtual async Task InitializeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.Database.EnsureCreatedAsync();

        var seeder = Provider.GetRequiredService<IDataSeeder>();
        await seeder.SeedAsync(CancellationToken.None);
    }

    public Task DisposeAsync() => Task.CompletedTask;
    
    public void Dispose()
    {
        ( _provider as IDisposable )?.Dispose();
    }
}