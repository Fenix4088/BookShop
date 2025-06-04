namespace BookShop.Domain.Entities.Rating;

public class BookRatingEntity : RatingBaseEntity
{
    public int BookId { get; private set; }
    public BookEntity Book { get; private set; }
}