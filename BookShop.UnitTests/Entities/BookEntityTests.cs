using System;
using BookShop.Domain;
using BookShop.Domain.Entities;
using BookShop.Domain.Entities.Rating;
using Shouldly;
using Xunit;

namespace BookShop.UnitTests.Entities;

public class BookEntityTests
{

    [Fact]
    public void Create_Book_Should_Create_Book_With_Correct_Values()
    {
        // Arrange
        var authorId = 1;
        var title = "Test Book";
        var description = "Test Description";
        var releaseDate = DateTime.Now;

        // Act
        var book = CreateBook(authorId, title, description, releaseDate);

        // Assert
        title.ShouldBe(book.Title);
        description.ShouldBe(book.Description);
        book.ReleaseDate.ShouldBe(releaseDate);
        book.AuthorId.ShouldBe(authorId);
        book.CreatedAt.ShouldBeLessThanOrEqualTo(DateTime.Now);
        book.IsDeleted.ShouldBeFalse();
        book.DeletedAt.ShouldBeNull();
        book.Ratings.ShouldNotBeNull();
        book.Ratings.Count.ShouldBe(0);
        book.CoverImgUrl.ShouldBeEmpty();
    }

    [Fact]
    public void Update_Book_Should_Update_Book_With_Correct_Values()
    {
        // Arrange
        var author = AuthorEntity.Create("Author", "Surname");
        var book = CreateBook(authorId: author.Id, title: "Old Title", description: "Old Description");

        var newTitle = "New Title";
        var newDescription = "New Description";
        var newReleaseDate = DateTime.Now.AddDays(1);

        // Act
        book.Update(author, newTitle, newDescription, newReleaseDate);

        // Assert
        book.Title.ShouldBe(newTitle);
        book.Description.ShouldBe(newDescription);
        book.ReleaseDate.ShouldBe(newReleaseDate);
        book.AuthorId.ShouldBe(author.Id);
    }
    
    [Fact]
    public void SoftDeleteRatings_Should_SoftDelete_All_Ratings()
    {
        // Arrange
        var book = CreateBook();
        book.Ratings.Add(BookRatingEntity.Create(1, Guid.NewGuid(), 5));
        book.Ratings.Add(BookRatingEntity.Create(2, Guid.NewGuid(), 4));

        // Act
        book.SoftDeleteRatings();

        // Assert
        foreach (var rating in book.Ratings)
        {
            rating.DeletedAt.ShouldNotBeNull();
        }
    }
    
    [Fact]
    public void SetCoverImage_Should_Set_CoverImageUrl()
    {
        // Arrange
        var book = CreateBook();
        var coverImageUrl = "http://example.com/cover.jpg";

        // Act
        book.SetCoverImage(coverImageUrl);

        // Assert
        book.CoverImgUrl.ShouldBe(coverImageUrl);
    }
    
    [Fact]
    public void SoftDelete_Should_Set_IsDeleted_True()
    {
        // Arrange
        var book = CreateBook();

        // Act
        book.SoftDelete();

        // Assert
        book.IsDeleted.ShouldBeTrue();
        book.DeletedAt.ShouldNotBeNull();
    }

    [Fact]
    public void Restore_Should_Set_IsDeleted_False()
    {
        // Arrange
        var book = CreateBook();
        book.SoftDelete();

        // Act
        book.Restore();

        // Assert
        book.IsDeleted.ShouldBeFalse();
        book.DeletedAt.ShouldBeNull();
    }
    

    private BookEntity CreateBook(int authorId = 1, string title = "TestBook", string description = "TestDescription", DateTime releaseDate = default)
    {
        return BookEntity.Create(
            title: title,
            description: description,
            releaseDate: releaseDate == default ? DateTime.Now : releaseDate,
            authorId: authorId
        );
    }
}