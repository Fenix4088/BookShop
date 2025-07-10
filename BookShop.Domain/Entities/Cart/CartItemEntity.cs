using System;

namespace BookShop.Domain.Entities.Cart;

public class CartItemEntity : BookShopGenericEntity
{
    public Guid Id { get; private set; }
    
    public Guid CartId { get; private set; }
    public CartEntity Cart { get; private set; }
    
    public int BookId { get; private set; }
    public BookEntity Book { get; private set; }
    
    public int Quantity { get; private set; }

    public bool IsBookDeleted { get; set; }
    public bool NotificationShown { get; set; }
    
    public static CartItemEntity Create(Guid cartId, int bookId, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        
        return new CartItemEntity
        {
            Id = Guid.NewGuid(),
            CartId = cartId,
            BookId = bookId,
            Quantity = quantity,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void IncreaseQuantity(int count = 1)
    {
        if (count <= 0) throw new ArgumentException("Count must be greater than zero.", nameof(count));
        
        Quantity += count;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void DecreaseQuantity(int count = 1)
    {
        if (count <= 0) throw new ArgumentException("Count must be greater than zero.", nameof(count));
        
        if (Quantity < count) throw new InvalidOperationException("Cannot decrease quantity below zero.");
        
        Quantity -= count;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(CartItemEntity item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        
        CartId = item.CartId;
        BookId = item.BookId;
        Quantity = item.Quantity;
        UpdatedAt = DateTime.UtcNow;
    }
    
}