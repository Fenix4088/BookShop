using BookShop.Application.Abstractions;

namespace BookShop.Application.Commands;

public record CreateAuthorCommand(string Name, string Surname) : ICommand;