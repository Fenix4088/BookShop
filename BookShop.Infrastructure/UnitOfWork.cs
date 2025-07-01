using System;
using System.Threading.Tasks;
using BookShop.Infrastructure.Abstractions;
using BookShop.Infrastructure.Context;
using Microsoft.Extensions.Logging;

namespace BookShop.Infrastructure;

internal sealed class UnitOfWork: IUnitOfWork
{
    private readonly ShopDbContext shopDbContext;
    private readonly ILogger<IUnitOfWork> logger;

    public UnitOfWork(ShopDbContext shopDbContext, ILogger<IUnitOfWork> logger)
    {
        this.shopDbContext = shopDbContext;
        this.logger = logger;
    }

    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = await shopDbContext.Database.BeginTransactionAsync();
        logger.LogInformation($"Starting transaction: {transaction.TransactionId} for unit of work.");

        try
        {
            await action();
            await shopDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            logger.LogInformation($"UoW committed transaction {transaction.TransactionId}");
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            logger.LogError($"UoW rolled back transaction {transaction.TransactionId} due to error: {e.Message}");
            throw;
        }
    }
}