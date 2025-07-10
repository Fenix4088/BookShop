using BookShop.Application.Abstractions;
using BookShop.Application.Models;
using BookShop.Domain.Repositories;
using BookShop.Shared.Pagination;
using BookShop.Shared.Pagination.Abstractions;

namespace BookShop.Application.Queries.Handlers;

public sealed class GetCartItemsQueryHandler(ICartRepository cartRepository) : IQueryHandler<GetCartItemsQuery, IPagedResult<CartItemModel>>
{
    public async Task<IPagedResult<CartItemModel>> Handler(GetCartItemsQuery query)
    {
        var pagedCartItems = await cartRepository.GetCartItemsPagedResultAsync(query, query.UserId);
        
        return new PagedResultModel<CartItemModel>()
        {
            Items = pagedCartItems.Items.Select(book => book.ToModel()).ToList(),
            PageSize = pagedCartItems.PageSize,
            TotalRowCount = pagedCartItems.TotalRowCount,
            PageCount = pagedCartItems.PageCount,
            CurrentPage = pagedCartItems.CurrentPage, 
        };
    }
}