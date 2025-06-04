using BookShop.Domain;
using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Context.Configurations;

public class BookEntityConfiguration: IEntityTypeConfiguration<BookEntity>
{
    protected string Schema { get; set; }

    public BookEntityConfiguration(string schema)
    {
        Schema = schema;
    }

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
        
        builder.HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(e => new { e.AuthorId, e.Title, e.ReleaseDate }).IsUnique();
    }
}