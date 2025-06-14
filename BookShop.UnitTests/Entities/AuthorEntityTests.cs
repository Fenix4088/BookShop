using BookShop.Domain;
using Shouldly;
using Xunit;

namespace BookShop.UnitTests.Entities;

public class AuthorEntityTests
{
    
    [Fact]
    public void CreateAuthor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var name = "Name";
        var surname = "Surname";

        // Act
        var author = CreateAuthor(name, surname);

        // Assert
        name.ShouldBe(author.Name);
        surname.ShouldBe(author.Surname);
        author.BookCount.ShouldBe(0);
        author.IsDeleted.ShouldBeFalse();
        author.Books.ShouldNotBeNull();
        author.Ratings.ShouldNotBeNull();
        author.Ratings.Count.ShouldBe(0);
        author.Books.Count.ShouldBe(0);
    }
    
    [Fact]
    public void UpdateAuthor_ShouldUpdateProperties()
    {
        // Arrange
        var author = CreateAuthor();
        var newName = "NewName";
        var newSurname = "NewSurname";

        // Act
        author.Update(newName, newSurname);

        // Assert
        author.Name.ShouldBe(newName);
        author.Surname.ShouldBe(newSurname);
    }
    
    [Fact]
    public void SoftDelete_ShouldSetIsDeletedTrue()
    {
        // Arrange
        var author = CreateAuthor();

        // Act
        author.SoftDelete();

        // Assert
        author.IsDeleted.ShouldBeTrue();
        author.DeletedAt.ShouldNotBeNull();
    }
    
    [Fact]
    public void Restore_ShouldSetIsDeletedFalse()
    {
        // Arrange
        var author = CreateAuthor();
        author.SoftDelete();

        // Act
        author.Restore();

        // Assert
        author.IsDeleted.ShouldBeFalse();
        author.DeletedAt.ShouldBeNull();
    }
    
    [Fact]
    public void AddBook_ShouldIncreaseBookCount()
    {
        // Arrange
        var author = CreateAuthor();

        // Act
        author.AddBook();

        // Assert
        author.BookCount.ShouldBe(1);
    }
    
    [Fact]
    public void RemoveBook_ShouldDecreaseBookCount()
    {
        // Arrange
        var author = CreateAuthor();
        author.AddBook();

        // Act
        author.RemoveBook();

        // Assert
        author.BookCount.ShouldBe(0);
    }
    

    private AuthorEntity CreateAuthor(string name = "TestName", string surname = "TestSurname")
    {
        return AuthorEntity.Create(name, surname);
    }
}