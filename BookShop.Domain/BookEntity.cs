using System;

namespace BookShop.Domain;

public class BookEntity : BookShopGenericEntity
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }  //Max 500 chars
    public DateTime ReleaseDate { get; private set; }
    public int AuthorId { get; private set; }

    public string? CoverImgUrl { get; private set; }
    public AuthorEntity Author { get; private set; }

    public static BookEntity Create(string title, string description, DateTime releaseDate, int authorId,
        string? coverImageUrl) => new()
    {
        Title = title,
        Description = description,
        ReleaseDate = releaseDate,
        AuthorId = authorId,
        CoverImgUrl = coverImageUrl,
        CreatedAt = DateTime.Now
    };

    public void SetCoverImage(string imageUrl)
    {
        CoverImgUrl = imageUrl;
    }

}