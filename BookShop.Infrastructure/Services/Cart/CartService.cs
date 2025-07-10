using System;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Application.Services;
using BookShop.Application.Users;
using BookShop.Domain.Entities.Cart;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Services.Cart;

public class CartService(
    ICartRepository cartRepository, 
    IUserRepository userRepository, 
    IBookRepository bookRepository, 
    ShopDbContext context
    ) : ICartService
{
    public async Task<CartEntity> CreateCartByUserIdAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if ( user is null)
        {
            throw new UserNotFoundException(userId);
        }

        return await cartRepository.CreateCartByUserIdAsync(userId);
    }

    public async Task AddItemToCartAsync(Guid userId, int bookId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if ( user is null)
        {
            throw new UserNotFoundException(userId);
        }
        
        var book = await bookRepository.GetBookById(bookId);
        
        if (book is null)
        {
            throw new BookNotFoundException(bookId);
        }
        
        var cart = await cartRepository.GetCartByUserIdAsync(userId);

        //! Predicate in case if cart was not created yet, for existing user
        if (cart is null)
        {
            cart = await CreateCartByUserIdAsync(userId);
        }
        
        var isNewCartItem = cart.IsAlreadyItemExists(bookId);

        var cartItem = cart.AddItem(book.Id);
        
        book.DecreaseQuantity();
        
        if (isNewCartItem)
        {
            context.CartItems.Update(cartItem);
        }
        else
        {
            await context.CartItems.AddAsync(cartItem);
        }

        await cartRepository.UpdateAsync(cart);
        await context.SaveChangesAsync();
    }

    public async Task RemoveItemFromCartAsync(Guid cartId, Guid cartItemId)
    {
        var cart = await cartRepository.GetCartByIdAsync(cartId);
        
        if (cart is null)
        {
            throw new CartNotFoundException(cartId);
        }
        
        var cartItem = cart.Items.SingleOrDefault(cartItem => cartItem.Id == cartItemId);
        
        if (cartItem is null)
        {
            throw new CartItemNotFoundException(cartItemId);
        }
        
        
        cartItem.Book.IncreaseQuantity();
        
        var isSingleItem = cartItem.Quantity == 1;
        
        cart.RemoveItem(cartItem);

        if (isSingleItem)
        {
            context.CartItems.Remove(cartItem);
        }
        else
        {
            context.CartItems.Update(cartItem);
        }
        
        await bookRepository.UpdateAsync(cartItem.Book);
        await cartRepository.UpdateAsync(cart);
        await context.SaveChangesAsync();
    }

    public async Task MarkNotificationShown(Guid cartId)
    {
        
        var cart = await cartRepository.GetCartByIdAsync(cartId);
        
        if (cart is null)
        {
            throw new CartNotFoundException(cartId);
        }
        
        await context.CartItems
            .Where(cartItem => 
                cartItem.CartId == cartId && 
                cartItem.IsBookDeleted == true && 
                cartItem.NotificationShown == false)
            .ExecuteUpdateAsync(updates => 
                updates.SetProperty(cartItem => cartItem.NotificationShown, _ => true));
    }
}