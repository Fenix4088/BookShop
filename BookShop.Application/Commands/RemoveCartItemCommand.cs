
using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record RemoveCartItemCommand(Guid CartId, Guid CartItemId) : ICommand;