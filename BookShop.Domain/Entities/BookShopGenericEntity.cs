using System;

namespace BookShop.Domain.Entities;

public abstract class BookShopGenericEntity
{
    public DateTime CreatedAt { get; protected set; }
    public DateTime? DeletedAt { get; private set; }

    public bool IsDeleted => DeletedAt.HasValue;

    public virtual void Delete()
    {
        DeletedAt = DateTime.UtcNow;
    }

    public virtual void Restore()
    {
        DeletedAt = null;
    }
}