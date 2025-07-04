using System;
using System.Collections.Generic;

namespace BookShop.Domain.Entities.Cart;

public sealed class CartEntity
{
    public Guid Id { get; private set; }
    
    public Guid? UserId { get; private set; }
    public Guid? GuestId { get; private set; }
    
    public ICollection<CartItemEntity> Items { get; private set; } = new List<CartItemEntity>();
    
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
}