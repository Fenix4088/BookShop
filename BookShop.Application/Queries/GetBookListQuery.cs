using BookShop.Application.Models;
using BookShop.Models.Queries.Abstractions;

namespace BookShop.Application.Queries;

public record GetBookListQuery(int CurrentPage, int RowCount) : IPagedQuery<BookModel>;