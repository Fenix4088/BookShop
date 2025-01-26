using System;
using System.Threading.Tasks;
using BookShop.Infrastructure.Abstractions;
using BookShop.Infrastructure.Context;

namespace BookShop.Infrastructure;

internal sealed class UnitOfWork: IUnitOfWork
{
    private readonly ShopDbContext _shopDbContext;

    public UnitOfWork(ShopDbContext shopDbContext)
    {
        _shopDbContext = shopDbContext;
    }

    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = await _shopDbContext.Database.BeginTransactionAsync();

        try
        {
            await action();
            await _shopDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}