using BookShop.Application.Models;
using BookShop.Models.Queries.Abstractions;

namespace BookShop.Application.Queries;

public record GetBookQuery(int BookId): IQuery<BookModel>;