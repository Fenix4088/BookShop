using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using BookShop.Domain;
using BookShop.Domain.Entities;
using BookShop.Domain.Entities.Cart;
using BookShop.Infrastructure.Abstractions;
using BookShop.Infrastructure.Identity;
using BookShop.Shared;
using BookShop.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookShop.Infrastructure.Context;

public class DataSeeder : IDataSeeder
{

    private readonly ShopDbContext ctx;
    private readonly UserManager<BookShopUser> userManager;
    private readonly RoleManager<BookShopRole> roleManager;
    private readonly ILogger<DataSeeder> logger;
    
    public DataSeeder(
        ShopDbContext ctx,
        UserManager<BookShopUser> userManager,
        RoleManager<BookShopRole> roleManager,
        ILogger<DataSeeder> logger)
    {
        this.ctx = ctx;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureCreatedAsync(cancellationToken);
        
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
            logger.LogInformation("✅ Database migration & seeding completed successfully.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "❌ Exception during database seeding");
            throw;
        }
    }


    private async Task EnsureCreatedAsync(CancellationToken cancellationToken = default)
    {
        if (ctx.Database.IsRelational())
        {
            logger.LogInformation("Applying relational migrations…");
            await ctx.Database.MigrateAsync(cancellationToken);
        }
        else
        {
            logger.LogInformation("Creating in-memory database schema…");
            await ctx.Database.EnsureCreatedAsync(cancellationToken);
        }
    }

    private async Task SeedRoles(RoleManager<BookShopRole> roleManager, CancellationToken cancellationToken)
    {
        logger.LogInformation("Seeding roles...");
        
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
        logger.LogInformation("Seeding users...");

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
        logger.LogInformation("Seeding authors...");
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

        logger.LogInformation("Seeding books...");

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
                .RuleFor(book => book.Quantity, f => f.Random.Int(100, 200))
                .RuleFor(book => book.Price, f => f.Finance.Amount(10, 100))
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
            CreateUserCart(user);
            await ConfirmEmail(user, userManager);
        }
    }

    private void CreateUserCart(BookShopUser user)
    {
        if (user.Cart != null)
        {
            return;
        }
        
        var cart = user.AddCart();

        if (cart is not null)
        {
            logger.LogInformation($"🛒 Cart created for user {user.UserName} with ID {user.Id}");
            ctx.Carts.Add(cart);
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