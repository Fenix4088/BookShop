using System;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Domain.Entities.Rating;

public abstract class RatingBaseEntity : BookShopGenericEntity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; protected set; }

    private int _score;
    [Range(1,5)]
    public int Score
    {
        get { return _score; }
        protected set
        {
            CheckScore(value);
            _score = value;
        }
    }
    
    protected virtual void CheckScore(int score)
    {
        if (score < 1 || score > 5)
            throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 1 and 5.");
    }
    
    
    public virtual void Update(int score)
    {
        CheckScore(score);
        
        Score = score;
        CreatedAt = DateTime.Now;
    }
}