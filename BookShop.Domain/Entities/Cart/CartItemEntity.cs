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
}