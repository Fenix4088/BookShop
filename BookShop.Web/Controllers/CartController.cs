using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Infrastructure.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;

public class CartController(
    IUserService userService,
    ICommandHandler<AddBookIntoCartCommand> addBookIntoCartCommandHandler
    ) : Controller
{
    [HttpGet]
    public IActionResult CartItems()
    {
        // This method should return the view for the cart items.
        // You would typically fetch the cart items from a service and pass them to the view.
        // For now, we will just return a placeholder view.
        //TODO: get and render cart items
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart([FromForm] int bookId)
    {
        var user = await userService.GetCurrentUserAsync();

        await addBookIntoCartCommandHandler.Handler(new AddBookIntoCartCommand(user.Id, bookId));
        
        return RedirectToAction("BooksList", "Books");
    }
}