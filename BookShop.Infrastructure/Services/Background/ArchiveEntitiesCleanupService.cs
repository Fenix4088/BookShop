using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookShop.Infrastructure.Services.Background;

internal sealed class ArchiveEntitiesCleanupService : BackgroundService
{

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ArchiveEntitiesCleanupService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromDays(1); // runs once a day

    public ArchiveEntitiesCleanupService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ArchiveEntitiesCleanupService> logger
        )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
                await CleanupOldArchivedEntities(dbContext);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during cleanup of archived records");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task CleanupOldArchivedEntities(ShopDbContext dbContext)
    {
        var cutoffDate = DateTime.UtcNow.AddYears(-1);

        var oldAuthors = await dbContext.Authors.Where(author => author.DeletedAt != null && author.DeletedAt < cutoffDate).ToListAsync();

        var oldBooks = await dbContext.Books.Where(book => book.DeletedAt != null && book.DeletedAt < cutoffDate).ToListAsync();

        if (oldAuthors.Any() || oldBooks.Any())
        {
            dbContext.Authors.RemoveRange(oldAuthors);
            dbContext.Books.RemoveRange(oldBooks);
            await dbContext.SaveChangesAsync();
            
            _logger.LogInformation($"Removed {oldAuthors.Count} authors and {oldBooks.Count} books, which were archived more than year");
        }
    }
}