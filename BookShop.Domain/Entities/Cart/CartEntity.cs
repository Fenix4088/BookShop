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


    public static CartEntity Create(Guid userId, bool isGuest = false) => isGuest ? new () {
        GuestId = userId,
        CreatedAt = DateTime.UtcNow,
    } : new()
    {
        UserId = userId,
        CreatedAt = DateTime.UtcNow,
    };
    
    
    public void AddItem(CartItemEntity item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        
        Items.Add(item);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void RemoveItem(CartItemEntity item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        
        Items.Remove(item);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Clear()
    {
        Items.Clear();
        UpdatedAt = DateTime.UtcNow;
    }
    
    
    public void Update(CartEntity cart)
    {
        if (cart == null) throw new ArgumentNullException(nameof(cart));
        
        UserId = cart.UserId;
        GuestId = cart.GuestId;
        Items = cart.Items;
        UpdatedAt = DateTime.UtcNow;
    }

}