using System;
using System.Collections.Generic;
using BookShop.Domain.Entities.Rating;

namespace BookShop.Domain.Entities;

public class BookEntity : BookShopGenericEntity
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }  //Max 500 chars
    public DateTime ReleaseDate { get; private set; }
    public int AuthorId { get; private set; }

    public string? CoverImgUrl { get; private set; }

    public int Count { get; private set; } = 0;
    
    public decimal Price { get; private set; } = 0;
    public AuthorEntity Author { get; private set; }
    
    public ICollection<BookRatingEntity> Ratings { get; private set; } = new List<BookRatingEntity>();

    
    public static BookEntity Create(string title, string description, DateTime releaseDate, int authorId, int count, decimal price, string? coverImageUrl = "") => new()
    {
        Title = title,
        Description = description,
        ReleaseDate = releaseDate,
        AuthorId = authorId,
        Count = count,
        Price = price,
        CoverImgUrl = coverImageUrl,
        CreatedAt = DateTime.Now
    };

    public void Update(AuthorEntity newAuthor, string title, string description, int count, decimal price, DateTime releaseDate)
    {
        Title = title;
        Description = description;
        ReleaseDate = releaseDate;
        Count = count;
        Price = price;

        if (AuthorId != newAuthor.Id)
        {
            Author.RemoveBook();
            AuthorId = newAuthor.Id;
            newAuthor.AddBook();
        }
    }

    public void SoftDeleteRatings()
    {
        foreach (var rating in Ratings)
        {
            rating.SoftDelete();
        }
    }

    public void SetCoverImage(string imageUrl)
    {
        CoverImgUrl = imageUrl;
    }

}