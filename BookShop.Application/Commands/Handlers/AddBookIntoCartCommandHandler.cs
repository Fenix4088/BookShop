using BookShop.Application.Abstractions;
using BookShop.Application.Services;

namespace BookShop.Application.Commands.Handlers;

public class AddBookIntoCartCommandHandler(ICartService cartService) : ICommandHandler<AddBookIntoCartCommand>
{
    public async Task Handler(AddBookIntoCartCommand command)
    {
        await cartService.AddItemToCartAsync(command.UserId, command.BookId);
    }
}