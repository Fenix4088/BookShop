using BookShop.Application.Abstractions;
using BookShop.Application.Services;

namespace BookShop.Application.Commands.Handlers;

public class RemoveCartItemCommandHandler(ICartService cartService) : ICommandHandler<RemoveCartItemCommand>
{
    public async Task Handler(RemoveCartItemCommand command)
    {
        await cartService.RemoveItemFromCartAsync(command.CartId, command.CartItemId);
    }
}