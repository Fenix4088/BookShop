using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using BookShop.Domain;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Identity;
using BookShop.Shared;
using BookShop.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        _logger.LogInformation("🚀 Starting database migration & seeding…");

        using var scope = _services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<ShopDbContext>();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<BookShopUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<BookShopRole>>();

        try
        {
            await ctx.Database.MigrateAsync(cancellationToken);
        
        
            if (!(await ctx.Users.AnyAsync(cancellationToken)))
            {
                await SeedRoles(roleManager, cancellationToken);
                await SeedUsers(userManager, cancellationToken);
            }

            if (!(await ctx.Authors.AnyAsync(cancellationToken)))
            {
                await SeedAuthors(ctx, cancellationToken);
                await SeedBooks(ctx, cancellationToken);
            }
            _logger.LogInformation("✅ Database migration & seeding completed successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception(e.Message, e);
        }
        

    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    
    
    private async Task SeedRoles(RoleManager<BookShopRole> roleManager, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Seeding roles...");
        
        var roles = new List<BookShopRole>
        {
            new() { Name = Roles.Admin.GetName() },
            new() { Name = Roles.User.GetName() },
            new() { Name = Roles.Manager.GetName() }
        };
        
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name))
            {
                await roleManager.CreateAsync(new BookShopRole { Name = role.Name });
            }
        }
    }

    private async Task SeedUsers(UserManager<BookShopUser> userManager, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Seeding users...");

        var adminEmail = "admin@bookshop.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new BookShopUser() { UserName = adminEmail, Email = adminEmail };
            await userManager.CreateAsync(adminUser, "Admin123!");
            await userManager.AddToRoleAsync(adminUser, Roles.Admin.GetName());
            await ConfirmEmail(adminUser, userManager);
            await GenerateUsers(userManager, Roles.Manager, 3);
            await GenerateUsers(userManager);
        }
    }

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
    

    private async Task GenerateUsers(UserManager<BookShopUser> userManager, Roles roles = Roles.User, int count = 5)
    {
        for  (int i = 0; i < count; i++)
        {
            var user = GenerateUser();
            await userManager.CreateAsync(user, "Test12345!");
            await userManager.AddToRoleAsync(user, roles.GetName());
            await ConfirmEmail(user, userManager);
        }
    }

    private BookShopUser GenerateUser() => new Faker<BookShopUser>()
        .RuleFor(user => user.UserName, f => f.Internet.Email())
        .RuleFor(user => user.Email, (f, u) => u.UserName);

    private async Task ConfirmEmail(BookShopUser user, UserManager<BookShopUser> userManager)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await userManager.ConfirmEmailAsync(user, token);
    }
}