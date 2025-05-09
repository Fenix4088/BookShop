﻿using BookShop.Domain;
using BookShop.Infrastructure.Context.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Context;

public class ShopDbContext : DbContext
{
    public const string Schema = "shop";

    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
    {
    }

    public DbSet<AuthorEntity> Authors { get; set; }
    public DbSet<BookEntity> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply custom table names for the entities
        modelBuilder.Entity<AuthorEntity>().ToTable("Authors", Schema);
        modelBuilder.Entity<BookEntity>().ToTable("Books", Schema);
        modelBuilder.ApplyConfiguration(new AuthorEntityConfiguration(Schema));
        modelBuilder.ApplyConfiguration(new BookEntityConfiguration(Schema));
    }
}