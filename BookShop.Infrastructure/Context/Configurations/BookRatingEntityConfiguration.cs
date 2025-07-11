using BookShop.Domain.Entities.Rating;
using BookShop.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Context.Configurations;

public class BookRatingEntityConfiguration(string schema) : IEntityTypeConfiguration<BookRatingEntity>
{
    protected string Schema { get; set; } = schema;

    public void Configure(EntityTypeBuilder<BookRatingEntity> builder)
    {
        builder.HasKey(bookRating => bookRating.Id);
        
        builder.HasIndex(bookRating => new { bookRating.UserId, bookRating.BookId })
            .IsUnique();
        
        builder.HasOne(bookRating => bookRating.Book)
            .WithMany(book => book.Ratings)
            .HasForeignKey(bookRating => bookRating.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // ? We use builder.HasOne<BookShopUser> instead builder.HasOne(bookRating => bookRating.User)
        // ? because BookRatingEntity (in Domain) doesn’t contain the User property.
        builder.HasOne<BookShopUser>()
            .WithMany(user => user.Ratings)
            .HasForeignKey(bookRating => bookRating.UserId);

        builder.Property(bookRating => bookRating.Score)
            .IsRequired()
            .HasDefaultValue(1);
        
        builder.Property(bookRating => bookRating.DeletedAt).IsRequired(false);

        builder.HasCheckConstraint("CK_BookRating_Score_Valid", "[Score] >= 1 AND [Score] <= 5");
    }
}