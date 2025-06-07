using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record RateBookCommand(int BookId, Guid UserId, int Score) : ICommand;