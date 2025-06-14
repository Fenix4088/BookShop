using System;
using BookShop.Application.Queries;
using BookShop.Domain;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Context;
using BookShop.Shared.Enums;

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

    public BookEntity CreateBook(AuthorEntity author, string title = "Test Title",
        string description = "Test Description")
    {
        var book = BookEntity.Create(title, description, DateTime.Now, author.Id);
        dbContext.Add(book);
        author.AddBook();
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

    public GetBookListQuery GenerateGetBookListQuery(
        int? currentPage = 1,
        int? pageSize = 10,
        SortDirection sortDirection = SortDirection.Descending,
        string searchByBookTitle = "",
        string searchByAuthorName = "",
        bool isDeleted = false) => new(currentPage ?? 1, pageSize ?? 10, sortDirection, searchByBookTitle,
        searchByAuthorName, isDeleted);

    public GetAuthorListQuery GenerateGetAuthorListQuery(
        int? currentPage = 1,
        int? pageSize = 10,
        SortDirection sortDirection = SortDirection.Descending,
        string searchByNameAndSurname = "",
        bool isDeleted = false) => new(currentPage ?? 1, pageSize ?? 10, sortDirection, searchByNameAndSurname,
        isDeleted);

}