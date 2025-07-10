using BookShop.Domain;
using BookShop.Domain.Entities;
using BookShop.Domain.Entities.Cart;

namespace BookShop.Application.Models;

public static class ModelsExtensions
{
    public static BookModel ToModel(this BookEntity bookEntity)
    {
        return new BookModel()
        {
            Id = bookEntity.Id,
            Title = bookEntity.Title,
            Description = bookEntity.Description,
            Quantity = bookEntity.Quantity,
            Price = bookEntity.Price,
            ReleaseDate = bookEntity.ReleaseDate,
            AuthorId = bookEntity.AuthorId,
            Author = bookEntity.Author.ToModel(),
            CoverImgUrl = bookEntity.CoverImgUrl,
            IsDeleted = bookEntity.IsDeleted,
            AverageRating = (int)Math.Round(bookEntity.Ratings.Count > 0 ? bookEntity.Ratings.Average(x => x.Score) : 0, 0),
        };
    }
    
    
    public static AuthorModel ToModel(this AuthorEntity authorEntity)
    {
        return new AuthorModel()
        {
            Id = authorEntity.Id,
            Name = authorEntity.Name,
            Surname = authorEntity.Surname,
            BookCount = authorEntity.BookCount,
            IsDeleted = authorEntity.IsDeleted,
            AverageRating = (int)Math.Round(authorEntity.Ratings.Count > 0 ? authorEntity.Ratings.Average(x => x.Score) : 0, 0),
        };
    }

    public static CartModel ToModel(this CartEntity cartEntity)
    {
        return new CartModel()
        {
            Id = cartEntity.Id,
            UserId = cartEntity.UserId,
            CartItems = cartEntity.Items.Select(x => x.ToModel()).ToList(),
            
        };
    }
    
    public static CartItemModel ToModel(this CartItemEntity cartItemEntity)
    {
        return new CartItemModel()
        {
            Id = cartItemEntity.Id,
            BookId = cartItemEntity.BookId,
            BookTitle = cartItemEntity.Book.Title,
            BookAuthorFullname = cartItemEntity.Book.Author.ToModel().NameAndSurname,
            Price = cartItemEntity.Book.Price,
            Quantity = cartItemEntity.Quantity,
            IsBookDeleted = cartItemEntity.IsBookDeleted,
            NotificationShown = cartItemEntity.NotificationShown,
        };
    }
}