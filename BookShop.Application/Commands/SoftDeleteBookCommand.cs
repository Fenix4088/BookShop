using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record SoftDeleteBookCommand(int BookId): ICommand;