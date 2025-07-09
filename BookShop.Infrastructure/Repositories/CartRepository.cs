using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Domain.Entities.Cart;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Repositories.Abstractions;
using BookShop.Shared.Pagination;
using BookShop.Shared.Pagination.Abstractions;
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
        return context.Carts
            .Include(cart => cart.Items)
            .ThenInclude(cartItem => cartItem.Book)
            .ThenInclude(book => book.Author)
            .FirstOrDefaultAsync(cart => cart.UserId == userId);
    }


    public Task<CartEntity> GetCartByIdAsync(Guid cartId)
    {
        return context.Carts
            .Include(cart => cart.Items)
            .ThenInclude(cartItem => cartItem.Book)
            .FirstOrDefaultAsync(cart => cart.Id == cartId);
    }

    public async Task<IEnumerable<CartItemEntity>> GetCartItemsAsync(Guid userId)
    {
        var cart = await GetCartByUserIdAsync(userId);
        return cart.Items;
    }

    public async Task<IEnumerable<CartItemEntity>> GetCartItemsByBookIdAsync(int bookId)
    {
        return await context.CartItems
            .Include(cartItem => cartItem.Book)
            .Where(cartItem => cartItem.BookId == bookId)
            .ToListAsync();
    }

    public async Task<IPagedResult<CartItemEntity>> GetCartItemsPagedResultAsync(IPagedQuery<CartItemEntity> pagedQuery, Guid userId)
    {

        var cartItems  = context.Carts
            .Where(cart => cart.UserId == userId)
            .SelectMany(cart => cart.Items)
            .OrderBy(cartItem => cartItem.CreatedAt);
        
        return await cartItems.ToPagedResult(pagedQuery, x => x);
    }
    
    
    public async Task MarkBookAsDeletedAsync(int bookId)
    {
        await context.CartItems
            .Where(cartItem => cartItem.BookId == bookId && !cartItem.IsBookDeleted)
            .ExecuteUpdateAsync(updates => updates
                .SetProperty(cartItem => cartItem.IsBookDeleted, _ => true)
                .SetProperty(cartItem => cartItem.NotificationShown, _ => false));
    }
}