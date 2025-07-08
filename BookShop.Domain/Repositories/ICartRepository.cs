using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookShop.Domain.Entities.Cart;
using BookShop.Shared.Pagination.Abstractions;

namespace BookShop.Domain.Repositories;

public interface ICartRepository : IRepository<CartEntity>
{
    Task<CartEntity> CreateCartByUserIdAsync(Guid userId);
    Task<CartEntity> GetCartByUserIdAsync(Guid userId, bool isGuest = false);
    Task<CartEntity> GetCartByIdAsync(Guid cartId);

    Task<IEnumerable<CartItemEntity>> GetCartItemsAsync(Guid userId);
    Task<IPagedResult<CartItemEntity>> GetCartItemsPagedResultAsync(IPagedQuery<CartItemEntity> pagedQuery, Guid userId);
}