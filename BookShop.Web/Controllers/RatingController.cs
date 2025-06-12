using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Infrastructure.Services.User;
using BookShop.Shared;
using BookShop.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;

public class RatingController : Controller
{
    
    private readonly ICommandHandler<RateBookCommand> rateBookCommandHandler;
    private readonly IUserService userService;
    
    
    
    public RatingController(
        ICommandHandler<RateBookCommand> rateBookCommandHandler, 
        IUserService userService
        )
    {
        this.rateBookCommandHandler = rateBookCommandHandler;
        this.userService = userService;
    }
    
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RateItem([FromForm] int currentPage, [FromForm] string itemType, [FromForm] int itemId, [FromForm] int score)
    {
        var user = await userService.GetCurrentUserAsync();
        
        if (itemType == RatingItemType.Book.GetName())
        {
            var command = new RateBookCommand(itemId, user.Id, score);
            await rateBookCommandHandler.Handler(command);
        }
        else
        {
        }

        return RedirectToAction("BooksList", "Books", new
        {
            CurrentPage = currentPage,
            RowCount = 10,
            IsDeleted = false
        });
    }
}