using BookShop.Application.Models;
using BookShop.Models.Queries.Abstractions;

namespace BookShop.Application.Queries;

public record GetAuthorQuery(int? Id): IQuery<AuthorModel>;