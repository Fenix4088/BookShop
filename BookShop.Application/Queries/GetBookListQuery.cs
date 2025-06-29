using BookShop.Application.Abstractions;
using BookShop.Shared.Enums;

namespace BookShop.Application.Queries;

public record GetBookListQuery(int CurrentPage, int RowCount, SortDirection SortDirection, string SearchByBookTitle, string SearchByAuthorName, bool IsDeleted) : IBookPageQuery;