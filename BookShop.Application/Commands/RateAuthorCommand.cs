using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record RateAuthorCommand(int AuthorId, Guid UserId, int Score) : ICommand;