
using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record SoftDeleteAuthorCommand(int AuthorId): ICommand;