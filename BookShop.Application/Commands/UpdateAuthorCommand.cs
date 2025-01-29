
using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record UpdateAuthorCommand(int Id, string Name, string Surname): ICommand;