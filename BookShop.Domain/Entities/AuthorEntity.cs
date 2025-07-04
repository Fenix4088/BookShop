﻿using System;
using System.Collections.Generic;
using BookShop.Domain.Entities;
using BookShop.Domain.Entities.Rating;

namespace BookShop.Domain;

public class AuthorEntity : BookShopGenericEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public int BookCount { get; private set; }
    
    public IReadOnlyCollection<BookEntity> Books => books;
    private List<BookEntity> books;
    
    public ICollection<AuthorRatingEntity> Ratings { get; private set; } = new List<AuthorRatingEntity>();

    public AuthorEntity()
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

    public override void SoftDelete()
    {
        base.SoftDelete();
        foreach (var book in books)
        {
            book.SoftDelete();
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

    public void AddBook(int count = 1)
    {
        BookCount += count;
    }
    
    public void RemoveBook()
    {
        if (BookCount <= 0) return;
        BookCount -= 1;
    }
    
    public void SoftDeleteRatings()
    {
        foreach (var rating in Ratings)
        {
            rating.SoftDelete();
        }
    }
    
}