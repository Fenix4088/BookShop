using System;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.Domain.Entities.Cart;

public sealed class CartEntity : BookShopGenericEntity
{
    public Guid Id { get; private set; }
    
    public Guid? UserId { get; private set; }
    public Guid? GuestId { get; private set; }
    
    public ICollection<CartItemEntity> Items { get; private set; } = new List<CartItemEntity>();
    

    public static CartEntity Create(Guid userId, bool isGuest = false) => isGuest ? new () {
        GuestId = userId,
        CreatedAt = DateTime.UtcNow,
    } : new()
    {
        UserId = userId,
        CreatedAt = DateTime.UtcNow,
    };
    
    
    public CartItemEntity AddItem(int bookId)
    {
        var existing = Items.FirstOrDefault(i => i.BookId == bookId);
        
        if (existing != null)
        {
            existing.IncreaseQuantity();
            UpdatedAt = DateTime.UtcNow;
            return existing;
        }
        
        var item = CartItemEntity.Create(Id, bookId, 1);
        Items.Add(item);
        UpdatedAt = DateTime.UtcNow;
        return item;
    }
    
    public bool IsAlreadyItemExists(int bookId)
    {
        return Items.Any(i => i.BookId == bookId);
    }
    
    public void RemoveItem(CartItemEntity item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        if (item.Quantity > 1)
        {
            item.DecreaseQuantity();
            UpdatedAt = DateTime.UtcNow;
            return;
        }

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