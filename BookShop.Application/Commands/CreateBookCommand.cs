using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record CreateBookCommand(int AuthorId, string Title, string Description, int Quantity, decimal Price, DateTime ReleaseDate): ICommand;