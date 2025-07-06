using System;
using System.Threading.Tasks;
using BookShop.Domain.Entities.Cart;

namespace BookShop.Infrastructure.Services.Cart;

public interface ICartService
{
    Task<CartEntity> CreateCartByUserIdAsync(Guid userId);
}