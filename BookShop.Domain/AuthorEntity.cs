using System;
using System.Collections.Generic;

namespace BookShop.Domain;

public class AuthorEntity : BookShopGenericEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public int BookCount { get; private set; }
    
    public IReadOnlyCollection<BookEntity> Books => books;
    private List<BookEntity> books;

    protected AuthorEntity()
    {
        books = new List<BookEntity>();
    }

    public static AuthorEntity Create(string name, string surname)
    {
        return new AuthorEntity
        {
            Name = name,
            Surname = surname,
            CreatedAt = DateTime.Now,
            BookCount = 0
        };
        
    }

    public override void Delete()
    {
        base.Delete();
        foreach (var book in books)
        {
            book.Delete();
        }
    }
    
    public override void Restore()
    {
        base.Restore();
        foreach (var book in books)
        {
            book.Restore();
        }
    }

    public void Update(string name, string surname)
    {
        Name = name;
        Surname = surname;
    }

    public void AddBook()
    {
        BookCount += 1;
    }
    
    public void RemoveBook()
    {
        if (BookCount <= 0) return;
        BookCount -= 1;
    }
}