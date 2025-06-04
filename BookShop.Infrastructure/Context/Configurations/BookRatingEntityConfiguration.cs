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
        throw new System.NotImplementedException();
    }
}