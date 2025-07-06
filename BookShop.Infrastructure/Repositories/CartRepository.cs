using System;
using System.Threading.Tasks;
using BookShop.Domain.Entities.Cart;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repositories;

public class CartRepository(ShopDbContext shopDbContext) : GenericRepository<CartEntity, ShopDbContext>(shopDbContext), ICartRepository
{
    public async Task<CartEntity> CreateCartByUserIdAsync(Guid userId)
    {
        var cart = await GetCartByUserIdAsync(userId);
        
        if (cart != null)
        {
            return cart;
        }
        
        var newCart = CartEntity.Create(userId);
        await context.Carts.AddAsync(newCart);
        await context.SaveChangesAsync();
        return newCart;
    }

    public Task<CartEntity> GetCartByUserIdAsync(Guid userId, bool isGuest = false)
    {
        return context.Carts.FirstOrDefaultAsync(cart => cart.UserId == userId);
    }


    public Task<CartEntity> GetCartByIdAsync(Guid cartId)
    {
        return context.Carts.FirstOrDefaultAsync(cart => cart.Id == cartId);
    }
    
}