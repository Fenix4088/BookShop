using System;
using System.Threading.Tasks;
using BookShop.Domain.Entities.Cart;
using BookShop.Domain.Repositories;

namespace BookShop.Infrastructure.Services.Cart;

public class CartService(ICartRepository cartRepository) : ICartService
{
    public async Task<CartEntity> CreateCartByUserIdAsync(Guid userId)
    {
        return await cartRepository.CreateCartByUserIdAsync(userId);
    }
}