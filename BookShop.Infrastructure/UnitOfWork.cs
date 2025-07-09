using System;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Abstractions;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Services.DomainEventDispatcher;
using Microsoft.Extensions.Logging;

namespace BookShop.Infrastructure;

internal sealed class UnitOfWork(ShopDbContext shopDbContext, ILogger<IUnitOfWork> logger, IDomainEventDispatcher dispatcher) : IUnitOfWork
{
    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = await shopDbContext.Database.BeginTransactionAsync();
        logger.LogInformation($"🔀 Starting transaction: {transaction.TransactionId} for unit of work. Target: {action.Target}");
        
        try
        {
            await action();
            await shopDbContext.SaveChangesAsync();

            await HandleDomainEvents();

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

    private async Task HandleDomainEvents()
    {
        var events = shopDbContext.ChangeTracker
            .Entries<Entity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var entry in shopDbContext.ChangeTracker
                     .Entries<Entity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        await dispatcher.DispatchAsync(events);
    }
}