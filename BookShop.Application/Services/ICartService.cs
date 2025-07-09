using BookShop.Domain.Entities.Cart;

namespace BookShop.Application.Services;

public interface ICartService
{
    Task<CartEntity> CreateCartByUserIdAsync(Guid userId);
    Task AddItemToCartAsync(Guid userId, int bookId);
    
    Task RemoveItemFromCartAsync(Guid cartId, Guid cartItemId);
}