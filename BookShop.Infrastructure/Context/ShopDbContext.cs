using System;
using BookShop.Domain;
using BookShop.Infrastructure.Context.Configurations;
using BookShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BookShop.Infrastructure.Context;

public class ShopDbContext : IdentityDbContext<BookShopUser, BookShopRole, Guid>
{
    public const string Schema = "shop";

    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
    {
    }

    public DbSet<AuthorEntity> Authors { get; set; }
    public DbSet<BookEntity> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<AuthorEntity>().ToTable("Authors", Schema);
        modelBuilder.Entity<BookEntity>().ToTable("Books", Schema);
        
        modelBuilder.ApplyConfiguration(new AuthorEntityConfiguration(Schema));
        modelBuilder.ApplyConfiguration(new BookEntityConfiguration(Schema));
    }
}