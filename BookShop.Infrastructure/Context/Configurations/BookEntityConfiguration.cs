using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Context.Configurations;

public class BookEntityConfiguration(string schema) : IEntityTypeConfiguration<BookEntity>
{
    protected string Schema { get; set; } = schema;

    public void Configure(EntityTypeBuilder<BookEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasDefaultValue(null)
            .HasMaxLength(500);
        
        builder.Property(e => e.ReleaseDate).IsRequired();
        
        builder.Property(author => author.DeletedAt).IsRequired(false);

        builder.Property(book => book.Quantity).IsRequired();
        
        builder.Property(book => book.Price).IsRequired().HasColumnType("decimal(18,2)");
        
        builder.HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(e => new { e.AuthorId, e.Title, e.ReleaseDate }).IsUnique();
    }
}