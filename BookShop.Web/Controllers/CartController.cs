using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Application.Queries.Handlers;
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
    IQueryHandler<GetCartQuery, CartModel> getCartQueryHandler
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
    public async Task<IActionResult> AddToCart([FromForm] int bookId)
    {
        var user = await userService.GetCurrentUserAsync();

        await addBookIntoCartCommandHandler.Handler(new AddBookIntoCartCommand(user.Id, bookId));
        
        return RedirectToAction("BooksList", "Books");
    }
}