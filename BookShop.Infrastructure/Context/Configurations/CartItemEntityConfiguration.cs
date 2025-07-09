using BookShop.Domain.Entities.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Context.Configurations;

public class CartItemEntityConfiguration(string schema) : IEntityTypeConfiguration<CartItemEntity>
{
    protected string Schema { get; set; } = schema;
    
    public void Configure(EntityTypeBuilder<CartItemEntity> builder)
    {
        builder.HasKey(cartItem => cartItem.Id);

        builder.Property(cartItem => cartItem.IsBookDeleted).HasDefaultValue(false);
        builder.Property(cartItem => cartItem.NotificationShown).HasDefaultValue(false);

        builder.HasOne(cartItem => cartItem.Cart)
            .WithMany(cart => cart.Items)
            .HasForeignKey(cart => cart.CartId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(cartItem => cartItem.Book)
            .WithMany(book => book.CartItems)
            .HasForeignKey(cartItem => cartItem.BookId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}