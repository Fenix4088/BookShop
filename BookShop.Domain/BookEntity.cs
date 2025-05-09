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

    
    //TODO: AddBook to author inside Create method
    public static BookEntity Create(string title, string description, DateTime releaseDate, int authorId,
        string? coverImageUrl = "") => new()
    {
        Title = title,
        Description = description,
        ReleaseDate = releaseDate,
        AuthorId = authorId,
        CoverImgUrl = "",
        CreatedAt = DateTime.Now
    };

    public void Update(int authorId, string title, string description, DateTime releaseDate)
    {
        Title = title;
        Description = description;
        ReleaseDate = releaseDate;
        AuthorId = authorId;

        if (AuthorId != authorId)
        {
            Author.RemoveBook();
            AuthorId = authorId;
            Author.AddBook();
        }
    }
    

    public void SetCoverImage(string imageUrl)
    {
        CoverImgUrl = imageUrl;
    }

}