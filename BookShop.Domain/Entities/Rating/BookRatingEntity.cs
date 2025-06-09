using System;

namespace BookShop.Domain.Entities.Rating;

public class BookRatingEntity : RatingBaseEntity
{
    public int BookId { get; private set; }
    public BookEntity Book { get; private set; }
    
    public static BookRatingEntity Create(int bookId, Guid userId, int score) => new()
    {
        BookId = bookId,
        UserId = userId,
        Score = score,
        CreateAt = DateTime.Now
    };
    
    
    public void Update(int score)
    {
        CheckScore(score);
        
        Score = score;
        CreateAt = DateTime.Now;
    }
    
}