using System;
using System.Collections.Generic;
using BookShop.Domain.Entities.Cart;
using BookShop.Domain.Entities.Rating;
using BookShop.Domain.Events;
using BookShop.Domain.Exceptions;

namespace BookShop.Domain.Entities;

public class BookEntity : BookShopGenericEntity
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }  //Max 500 chars
    public DateTime ReleaseDate { get; private set; }
    public int AuthorId { get; private set; }

    public string? CoverImgUrl { get; private set; }

    public int Quantity { get; private set; } = 0;
    
    public decimal Price { get; private set; } = 0;
    public AuthorEntity Author { get; private set; }
    
    public ICollection<BookRatingEntity> Ratings { get; private set; } = new List<BookRatingEntity>();
    
    public ICollection<CartItemEntity> CartItems { get; private set; } = new List<CartItemEntity>();
    
    public static BookEntity Create(string title, string description, DateTime releaseDate, int authorId, int quantity, decimal price, string? coverImageUrl = "") => new()
    {
        Title = title,
        Description = description,
        ReleaseDate = releaseDate,
        AuthorId = authorId,
        Quantity = quantity,
        Price = price,
        CoverImgUrl = coverImageUrl,
        CreatedAt = DateTime.Now
    };

    public void Update(AuthorEntity newAuthor, string title, string description, int quantity, decimal price, DateTime releaseDate)
    {
        Title = title;
        Description = description;
        ReleaseDate = releaseDate;
        Quantity = quantity;
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

    public void DecreaseQuantity(int count = 1)
    {
        if (count <= 0) throw new ArgumentException("Count must be greater than zero.", nameof(count));
        if (Quantity < count) throw new BookIsOutOfStockException(Id, Title);

        Quantity -= count;
    }
    
    public void IncreaseQuantity(int count = 1)
    {
        if (count <= 0) throw new ArgumentException("Count must be greater than zero.", nameof(count));
        Quantity += count;
    }

    public void SetCoverImage(string imageUrl)
    {
        CoverImgUrl = imageUrl;
    }

    public override void SoftDelete()
    {
        AddDomainEvent(new BookDeleteEvent(Id));
        base.SoftDelete();
    }
}