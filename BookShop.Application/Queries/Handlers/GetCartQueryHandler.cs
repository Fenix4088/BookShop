using BookShop.Application.Abstractions;
using BookShop.Application.Models;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Queries.Handlers;

public class GetCartQueryHandler(ICartRepository cartRepository) : IQueryHandler<GetCartQuery, CartModel>
{
    public async Task<CartModel> Handler(GetCartQuery query)
    {
        return (await cartRepository.GetCartByUserIdAsync(query.UserId)).ToModel();
    }
}