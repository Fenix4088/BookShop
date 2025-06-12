using BookShop.Application.Models;
using BookShop.Shared.Abstractions;

namespace BookShop.Application.Queries;

public record GetAuthorQuery(int? Id): IQuery<AuthorModel>;