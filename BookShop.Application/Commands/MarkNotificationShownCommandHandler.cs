using BookShop.Application.Abstractions;
using BookShop.Application.Services;

namespace BookShop.Application.Commands;

public class MarkNotificationShownCommandHandler(ICartService carService) : ICommandHandler<MarkNotificationShownCommand>
{
    public async Task Handler(MarkNotificationShownCommand command)
    {
        await carService.MarkNotificationShown(command.CartId);
    }
}