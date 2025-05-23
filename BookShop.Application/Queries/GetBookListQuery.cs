using BookShop.Application.Enums;
using BookShop.Application.Models;
using BookShop.Models.Queries.Abstractions;

namespace BookShop.Application.Queries;

public record GetBookListQuery(int CurrentPage, int RowCount, SortDirection SortDirection, string SearchByNameAndSurname) : IPagedQuery<BookModel>;