using BookShop.Domain.Entities.Cart;
using BookShop.Shared.Enums;
using BookShop.Shared.Pagination.Abstractions;

namespace BookShop.Application.Queries;

public record GetCartItemsQuery(Guid UserId, int CurrentPage, int RowCount, SortDirection SortDirection) : IPagedQuery<CartItemEntity>;