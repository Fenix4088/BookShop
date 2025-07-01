using System;
using BookShop.Domain.Entities.Rating;
using BookShop.UnitTests.Helpers;
using Shouldly;
using Xunit;

namespace BookShop.UnitTests.Entities;

public class BookRatingEntityTests
{
    [Fact]
    public void CreateBookRating_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var author = EntityHelpers.CreateAuthor();
        var book = EntityHelpers.CreateBook(author.Id);
        var user = EntityHelpers.CreateUser();

        // Act
        var bookRating = BookRatingEntity.Create(book.Id, user.Id, 5);

        // Assert
        bookRating.BookId.ShouldBe(book.Id);
        bookRating.UserId.ShouldBe(user.Id);
        bookRating.Score.ShouldBe(5);
        bookRating.DeletedAt.ShouldBeNull();
    }
    
    [Fact]
    public void UpdateBookRating_ShouldUpdateProperties()
    {
        // Arrange
        var author = EntityHelpers.CreateAuthor();
        var book = EntityHelpers.CreateBook(author.Id);
        var user = EntityHelpers.CreateUser();
        var bookRating = BookRatingEntity.Create(book.Id, user.Id, 5);

        // Act
        bookRating.Update(4);

        // Assert
        bookRating.Score.ShouldBe(4);
    }
    
    [Fact]
    public void UpdateBookRating_ShouldThrowException_WhenScoreIsInvalid()
    {
        // Arrange
        var author = EntityHelpers.CreateAuthor();
        var book = EntityHelpers.CreateBook(author.Id);
        var user = EntityHelpers.CreateUser();
        var bookRating = BookRatingEntity.Create(book.Id, user.Id, 5);

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => bookRating.Update(6));
        
        Should.Throw<ArgumentOutOfRangeException>(() => bookRating.Update(-1));
    }
    
    [Fact]
    public void SoftDelete_ShouldSetDeletedAt()
    {
        // Arrange
        var author = EntityHelpers.CreateAuthor();
        var book = EntityHelpers.CreateBook(author.Id);
        var user = EntityHelpers.CreateUser();
        var bookRating = BookRatingEntity.Create(book.Id, user.Id, 5);

        // Act
        bookRating.SoftDelete();

        // Assert
        bookRating.DeletedAt.ShouldNotBeNull();
    }
}