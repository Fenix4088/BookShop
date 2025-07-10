using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record MarkNotificationShownCommand(Guid CartId) : ICommand;