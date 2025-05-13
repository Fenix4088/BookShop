using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using BookShop.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookShop.Infrastructure.Context;

internal sealed class DatabaseInitializer: IHostedService
{
    
    private readonly IServiceProvider _services;
    private readonly ILogger<DatabaseInitializer> _logger;
    
    public DatabaseInitializer(IServiceProvider services, ILogger<DatabaseInitializer> logger)
    {
        _services = services;
        _logger   = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting database migration & seeding…");

        using var scope = _services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<ShopDbContext>();

        await ctx.Database.MigrateAsync(cancellationToken);

        if (!(await ctx.Authors.AnyAsync(cancellationToken)))
        {
            await SeedAuthors(ctx, cancellationToken);
            await SeedBooks(ctx, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task SeedAuthors(ShopDbContext ctx, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Seeding authors...");
        var authors = new Faker<AuthorEntity>()
            .RuleFor(author => author.Name, f => f.Name.FirstName())
            .RuleFor(author => author.Surname, f => f.Name.LastName())
            .RuleFor(author => author.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(author => author.DeletedAt, f => null)
            .RuleFor(author => author.BookCount, f => 0)
            .Generate(10);
            
        ctx.Authors.AddRange(authors);
        await ctx.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedBooks(ShopDbContext ctx, CancellationToken cancellationToken)
    {
        var seededAuthors = await ctx.Authors.ToListAsync(cancellationToken);

        _logger.LogInformation("Seeding books...");

        var books = new List<BookEntity>();
        foreach (var seededAuthor in seededAuthors)
        {
            books.AddRange(new Faker<BookEntity>()
                .RuleFor(book => book.Title, f => f.Lorem.Sentence(3))
                .RuleFor(book => book.Description, f => f.Lorem.Paragraph(2))
                .RuleFor(book => book.ReleaseDate, f => f.Date.Past(10))
                .RuleFor(book => book.AuthorId, seededAuthor.Id)
                .RuleFor(book => book.CoverImgUrl, f => f.Image.PicsumUrl())
                .RuleFor(book => book.CreatedAt, f => DateTime.UtcNow)
                .RuleFor(book => book.DeletedAt, f => null)
                .Generate(5));
            seededAuthor.AddBook(5);
        }
            
        ctx.Books.AddRange(books);

        await ctx.SaveChangesAsync(cancellationToken);
    }
}