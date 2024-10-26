using System;
using System.Collections.Generic;
using BookShop.Domain.Validators;
using FluentValidation;

namespace BookShop.Domain;

public class AuthorEntity
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
        var author = new AuthorEntity
        {
            Name = name,
            Surname = surname,
            BookCount = 0
        };
            
        new CreateAuthorValidation().ValidateAndThrow(author);
        return author;
    }

    public void AddBook(string title, string description, string releaseDate)
    {
        throw new NotImplementedException();
    }
}