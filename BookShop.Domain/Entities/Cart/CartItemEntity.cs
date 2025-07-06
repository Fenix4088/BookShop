using System;

namespace BookShop.Domain.Entities.Cart;

public class CartItemEntity
{
    public Guid Id { get; private set; }
    
    public Guid CartId { get; private set; }
    public CartEntity Cart { get; private set; }
    
    public int BookId { get; private set; }
    public BookEntity Book { get; private set; }
    
    public int Quantity { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime? UpdatedAt { get; private set; }
    
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
    
    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        
        Quantity = quantity;
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