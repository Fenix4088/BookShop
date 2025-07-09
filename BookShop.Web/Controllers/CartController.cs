using System;
using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Application.Services;
using BookShop.Infrastructure.Services.User;
using BookShop.Shared.Enums;
using BookShop.Shared.Pagination.Abstractions;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;

public class CartController(
    IUserService userService,
    ICartService cartService,
    ICommandHandler<AddBookIntoCartCommand> addBookIntoCartCommandHandler,
    IQueryHandler<GetCartItemsQuery, IPagedResult<CartItemModel>> getCartItemsQueryHandler,
    IQueryHandler<GetCartQuery, CartModel> getCartQueryHandler,
    IQueryHandler<GetBookListQuery, IPagedResult<BookModel>> getBookListQueryHandler,
    ICommandHandler<RemoveCartItemCommand> removeCartItemCommandHandler
    ) : Controller
{
    
    [HttpGet]
    public async Task<IActionResult> CartItems([FromQuery] PagedQueryModel model)
    {
        var user = await userService.GetCurrentUserAsync();

        var cart = await getCartQueryHandler.Handler(new GetCartQuery(user.Id));
        
        var cartItems = await getCartItemsQueryHandler.Handler(new GetCartItemsQuery(user.Id, model.CurrentPage, model.RowCount,
            SortDirection.Ascending));
        
        return View("CartItems", new CartDetailsViewModel()
        {
            Cart = cart,
            Items = cartItems
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart([FromForm] int currentPage, [FromForm] int bookId)
    {
        var user = await userService.GetCurrentUserAsync();

        await addBookIntoCartCommandHandler.Handler(new AddBookIntoCartCommand(user.Id, bookId));
        
        return await RedirectToBookList(currentPage);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart([FromForm] int currentPage, [FromForm] Guid cartId, [FromForm] Guid cartItemId)
    {
        await removeCartItemCommandHandler.Handler(new RemoveCartItemCommand(cartId, cartItemId));

        return RedirectToAction("CartItems");
    }


    private async Task<IActionResult> RedirectToBookList(int currentPage)
    {
        if (currentPage == 0)
        {
            return RedirectToAction("BooksList", "Books", new
            {
                CurrentPage = 1,
                RowCount = 10,
                IsDeleted = false
            });
        }
        return RedirectToAction("BooksList", "Books", await getBookListQueryHandler.Handler(new GetBookListQuery(currentPage, 10, SortDirection.Descending, "", "", false)));
    }
}