using BookShop.Application.Models;
using BookShop.Domain;

namespace BookShop.Web;

public static class Extensions
{
    public static BookModel ToModel(this BookEntity bookEntity)
    {
        return new BookModel()
        {
            Id = bookEntity.Id,
            Title = bookEntity.Title,
            Description = bookEntity.Description,
            ReleaseDate = bookEntity.ReleaseDate,
            AuthorId = bookEntity.AuthorId,
            Author = bookEntity.Author.ToModel(),
            CoverImgUrl = bookEntity.CoverImgUrl
        };
    }
    
    
    public static AuthorModel ToModel(this AuthorEntity authorEntity)
    {
        return new AuthorModel()
        {
            Id = authorEntity.Id,
            Name = authorEntity.Name,
            Surname = authorEntity.Surname,
        };
    }
}