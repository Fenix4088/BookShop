using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Application.Enums;
using BookShop.Infrastructure.Services.User;
using BookShop.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;

public class RatingController : Controller
{
    
    private readonly ICommandHandler<RateBookCommand> rateBookCommandHandler;
    private readonly IUserService userService;
    
    public RatingController(ICommandHandler<RateBookCommand> rateBookCommandHandler)
    {
        this.rateBookCommandHandler = rateBookCommandHandler;
        this.userService = userService;
    }
    
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RateItem([FromForm] string itemType, [FromForm] int itemId, [FromForm] int score)
    {
        var user = await userService.GetCurrentUserAsync();

        if (itemType == RatingItemType.Book.GetName())
        {
            var command = new RateBookCommand(itemId, user.Id, score);
            await rateBookCommandHandler.Handler(command);
            return Ok();
        }
        
        return Ok();

        // rateBookCommandHandler.Handler(new RateBookCommand())
    }
}