using BookShop.Domain.Entities.Rating;
using BookShop.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Context.Configurations;

public class AuthorRatingEntityConfiguration : IEntityTypeConfiguration<AuthorRatingEntity>
{
    
    protected string Schema { get; set; }
    
    public AuthorRatingEntityConfiguration(string schema)
    {
        Schema = schema;
    }
    
    public void Configure(EntityTypeBuilder<AuthorRatingEntity> builder)
    {
        builder.HasKey(authorRating => authorRating.Id);
        builder.HasIndex(authorRating => new { authorRating.UserId, authorRating.AuthorId })
            .IsUnique();
        
        builder.HasOne(authorRating => authorRating.Author)
            .WithMany(author => author.Ratings)
            .HasForeignKey(authorRating => authorRating.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<BookShopUser>()
            .WithMany(user => user.RatingsAuthor)
            .HasForeignKey(authorRating => authorRating.UserId);

        builder.Property(authorRating => authorRating.Score)
            .IsRequired()
            .HasDefaultValue(1);
        
        builder.Property(bookRating => bookRating.DeletedAt).IsRequired(false);
        
        builder.HasCheckConstraint("CK_AuthorRating_Score_Valid", "[Score] >= 1 AND [Score] <= 5");
    }
}