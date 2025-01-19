
using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record RemoveAuthorCommand(int AuthorId): ICommand;