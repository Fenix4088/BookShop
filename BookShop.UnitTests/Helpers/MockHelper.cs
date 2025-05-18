using System;
using BookShop.Domain;
using BookShop.Infrastructure.Context;

namespace BookShop.UnitTests.Helpers;

public sealed class MockHelper
{
    private readonly ShopDbContext dbContext;
    public MockHelper(ShopDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public AuthorEntity CreateAuthorWithBooks(string name = "Test Name", string surname = "Test Surname")
    {
        var author = AuthorEntity.Create(name, surname);
        dbContext.Add(author);
        dbContext.SaveChanges();

        for (int i = 0; i < 3; i++)
        {
            var book = BookEntity.Create($"Test Title {i}", $"Test Description {i}", DateTime.Now, author.Id);
            dbContext.Add(book);
        }
        
        dbContext.SaveChanges();
        return author;
    }
    
    public BookEntity CreateBook(AuthorEntity author, string title = "Test Title", string description = "Test Description")
    {
        var book = BookEntity.Create(title, description, DateTime.Now, author.Id);
        dbContext.Add(book);
        dbContext.SaveChanges();
        return book;
    }
    
    public AuthorEntity CreateAuthor(string name = "Test Name", string surname = "Test Surname")
    {
        var author = AuthorEntity.Create(name, surname);
        dbContext.Add(author);
        dbContext.SaveChanges();
        return author;
    }
}