using BookShop.Domain.Entities.Rating;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Context.Configurations;

public class BookRatingEntityConfiguration : IEntityTypeConfiguration<BookRatingEntity>
{
    protected string Schema { get; set; }

    public BookRatingEntityConfiguration(string schema)
    {
        Schema = schema;
    }

    public void Configure(EntityTypeBuilder<BookRatingEntity> builder)
    {
        builder.HasKey(bookRating => bookRating.Id);
        
        builder.HasIndex(bookRating => new { bookRating.UserId, bookRating.BookId })
            .IsUnique();
        
        builder.HasOne(bookRating => bookRating.Book)
            .WithMany(book => book.Ratings)
            .HasForeignKey(bookRating => bookRating.BookId);

        builder.HasOne(bookRating => bookRating.User)
            .WithMany(user => user.Ratings)
            .HasForeignKey(bookRating => bookRating.UserId);

        builder.Property(bookRating => bookRating.Score)
            .IsRequired()
            .HasDefaultValue(1);

        builder.HasCheckConstraint("CK_Rating_Score_Valid", "[Score] >= 1 AND [Score] <= 5");
    }
}