using System;
using BookShop.Domain;
using BookShop.Domain.Entities;
using BookShop.Domain.Entities.Rating;
using BookShop.Infrastructure.Context.Configurations;
using BookShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Context;

public class ShopDbContext : IdentityDbContext<BookShopUser, BookShopRole, Guid>
{
    public const string Schema = "shop";

    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
    {
    }

    public DbSet<AuthorEntity> Authors { get; set; }
    public DbSet<BookEntity> Books { get; set; }
    public DbSet<BookRatingEntity> BookRatings { get; set; }
    
    public DbSet<AuthorRatingEntity> AuthorRatings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<AuthorEntity>().ToTable("Authors", Schema);
        modelBuilder.Entity<BookEntity>().ToTable("Books", Schema);
        modelBuilder.Entity<BookRatingEntity>().ToTable("BookRatings", Schema);
        modelBuilder.Entity<AuthorRatingEntity>().ToTable("AuthorRatings", Schema);
        
        modelBuilder.ApplyConfiguration(new AuthorEntityConfiguration(Schema));
        modelBuilder.ApplyConfiguration(new BookEntityConfiguration(Schema));
        modelBuilder.ApplyConfiguration(new BookRatingEntityConfiguration(Schema));
        modelBuilder.ApplyConfiguration(new AuthorRatingEntityConfiguration(Schema));
    }
}