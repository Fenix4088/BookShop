using BookShop.Domain.Entities.Cart;
using BookShop.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Context.Configurations;

public class CartEntityConfiguration(string schema) : IEntityTypeConfiguration<CartEntity>
{
    protected string Schema { get; set; } = schema;
    
    public void Configure(EntityTypeBuilder<CartEntity> builder)
    {
        builder.HasKey(cart => cart.Id);

        builder.HasIndex(cart => cart.UserId).IsUnique();
        
        builder.HasOne<BookShopUser>()
            .WithOne(user => user.Cart)
            .HasForeignKey<CartEntity>(cart => cart.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}