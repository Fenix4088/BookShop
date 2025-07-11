using System;

namespace BookShop.Domain.Entities.Rating;

public class AuthorRatingEntity : RatingBaseEntity
{
    public int AuthorId { get; set; }
    public AuthorEntity Author { get; set; }
    
    public static AuthorRatingEntity Create(int authorId, Guid userId, int score) => new()
    {
        AuthorId = authorId,
        UserId = userId,
        Score = score,
        CreatedAt = DateTime.Now
    };
}