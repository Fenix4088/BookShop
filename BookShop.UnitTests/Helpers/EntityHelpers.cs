using System;
using BookShop.Domain;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Identity;

namespace BookShop.UnitTests.Helpers;

public static class EntityHelpers
{
    public static BookEntity CreateBook(int authorId, string title = "TestTitle", string description = "TestDescription", int quantity = 1, decimal price = 1, DateTime releaseDate = default) 
    {
        return BookEntity.Create(title, description, releaseDate, authorId, quantity, price);
    }
    public static AuthorEntity CreateAuthor(string name = "TestName", string surname = "TestSurname")
    {
        return AuthorEntity.Create(name, surname);
    }
    
    public static BookShopUser CreateUser(string userName = "testuser", string email = "")
    {
        return new BookShopUser
        {
            UserName = userName,
            Email = email,
            Id = Guid.NewGuid()
        };
    }
}