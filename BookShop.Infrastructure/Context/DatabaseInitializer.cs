using System;
using System.Threading;
using System.Threading.Tasks;
using BookShop.Infrastructure.Abstractions;
using BookShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookShop.Infrastructure.Context;

internal sealed class DatabaseInitializer: IHostedService
{
    
    private readonly IServiceProvider _services;
    private readonly ILogger<DatabaseInitializer> _logger;
    
    public DatabaseInitializer(
        IServiceProvider services, 
        ILogger<DatabaseInitializer> logger
        )
    {
        _services = services;
        _logger   = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("🚀 Starting database migration & seeding…");

            using var scope = _services.CreateScope();

            var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
            await seeder.SeedAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "❌ Exception during database seeding");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}