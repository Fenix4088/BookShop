using BookShop.Application.Models;
using BookShop.Shared.Abstractions;

namespace BookShop.Application.Queries;

public record GetBookQuery(int BookId): IQuery<BookModel>;