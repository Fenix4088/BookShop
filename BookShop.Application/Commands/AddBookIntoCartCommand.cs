
using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record AddBookIntoCartCommand(Guid UserId, int BookId) : ICommand;