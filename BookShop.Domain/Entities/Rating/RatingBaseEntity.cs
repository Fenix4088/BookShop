using System;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Domain.Entities.Rating;

public abstract class RatingBaseEntity
{
    public Guid Id { get; private set; }
    public int UserId { get; private set; }
    public UserEntity User { get; private set; }
    [Range(1,5)]
    public int Score { get; private set; }
    public DateTime RateAt { get; private set; }
}