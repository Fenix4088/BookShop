using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record UpdateBookCommand(int Id, int AuthorId, string Title, string Description, int Quantity, decimal Price, DateTime ReleaseDate): ICommand;