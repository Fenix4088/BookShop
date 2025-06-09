using System;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Domain.Entities.Rating;

public abstract class RatingBaseEntity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; protected set; }
    // public UserEntity User { get; protected set; }
    [Range(1,5)]
    public int Score { get; protected set; }
    public DateTime CreateAt { get; protected set; }
    
    
    protected virtual void CheckScore(int score)
    {
        if (score < 1 || score > 5)
            throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 1 and 5.");
    }
}