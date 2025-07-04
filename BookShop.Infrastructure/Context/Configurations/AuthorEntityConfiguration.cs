using BookShop.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Context.Configurations;

public class AuthorEntityConfiguration(string schema) : IEntityTypeConfiguration<AuthorEntity>
{
    protected string Schema { get; set; } = schema;

    public void Configure(EntityTypeBuilder<AuthorEntity> builder)
    {
        builder.HasKey(author => author.Id);
        builder.Property(author => author.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(author => new { author.Name, author.Surname, })
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");

        builder.Property(author => author.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(author => author.Surname)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(author => author.DeletedAt).IsRequired(false);

        builder.Metadata
            .FindNavigation(nameof(AuthorEntity.Books))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(author => author.Books)
            .WithOne(book => book.Author)
            .HasForeignKey(book => book.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}