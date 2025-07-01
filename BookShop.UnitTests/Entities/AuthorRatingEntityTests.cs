using System;
using BookShop.Domain.Entities.Rating;
using BookShop.UnitTests.Helpers;
using Shouldly;
using Xunit;

namespace BookShop.UnitTests.Entities;

public class AuthorRatingEntityTests
{
    

    [Fact]
    public void CreateAuthorRating_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var author = EntityHelpers.CreateAuthor();
        var user = EntityHelpers.CreateUser();
        var score = 5;

        // Act
        var authorRating = AuthorRatingEntity.Create(author.Id, user.Id, score);

        // Assert
        author.Id.ShouldBe(authorRating.AuthorId);
        authorRating.AuthorId.ShouldBe(author.Id);
        authorRating.UserId.ShouldBe(user.Id);
        authorRating.Score.ShouldBe(score);
    }

    [Fact]
    public void UpdateAuthorRating_ShouldUpdateProperties()
    {
        // Arrange
        var author = EntityHelpers.CreateAuthor();
        var user = EntityHelpers.CreateUser();
        var score = 5;
        var authorRating = AuthorRatingEntity.Create(author.Id, user.Id, score);

        var newScore = 4;

        // Act
        authorRating.Update(newScore);

        // Assert
        authorRating.Score.ShouldBe(newScore);
    }
    
    [Fact]
    public void UpdateAuthorRating_ShouldThrowException_WhenScoreIsInvalid()
    {
        // Arrange
        var author = EntityHelpers.CreateAuthor();
        var user = EntityHelpers.CreateUser();
        var score = 5;
        var authorRating = AuthorRatingEntity.Create(author.Id, user.Id, score);

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => authorRating.Update(6));
    }

    [Fact]
    public void UpdateAuthorRating_ShouldThrowException_WhenScoreIsLessThan_1()
    {
        // Arrange
        var author = EntityHelpers.CreateAuthor();
        var user = EntityHelpers.CreateUser();
        var score = 5;
        var authorRating = AuthorRatingEntity.Create(author.Id, user.Id, score);

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => authorRating.Update(0));
    }
    
    
    [Fact]
    public void SoftDelete_ShouldSetIsDeletedTrue()
    {
        // Arrange
        var author = EntityHelpers.CreateAuthor();
        var user = EntityHelpers.CreateUser();
        var score = 5;
        var authorRating = AuthorRatingEntity.Create(author.Id, user.Id, score);

        // Act
        authorRating.SoftDelete();

        // Assert
        authorRating.DeletedAt.ShouldNotBeNull();
    }
}