using System;
using System.Threading.Tasks;
using BookShop.Domain.Entities.Cart;

namespace BookShop.Domain.Repositories;

public interface ICartRepository : IRepository<CartEntity>
{
    Task<CartEntity> CreateCartByUserIdAsync(Guid userId);
    Task<CartEntity> GetCartByUserIdAsync(Guid userId, bool isGuest = false);
    Task<CartEntity> GetCartByIdAsync(Guid cartId);
}