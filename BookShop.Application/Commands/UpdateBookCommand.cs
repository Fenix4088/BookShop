using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record UpdateBookCommand(int Id, int AuthorId, string Title, string Description, int Count, decimal Price, DateTime ReleaseDate): ICommand;