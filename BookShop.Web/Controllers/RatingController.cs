using System;
using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Infrastructure.Services.User;
using BookShop.Shared;
using BookShop.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookShop.Web.Controllers;

public class RatingController : Controller
{
    
    private readonly ICommandHandler<RateBookCommand> rateBookCommandHandler;
    private readonly ICommandHandler<RateAuthorCommand> rateAuthorCommandHandler;
    private readonly IUserService userService;
    private readonly ILogger<RatingController> logger;
    
    public RatingController(
        ICommandHandler<RateBookCommand> rateBookCommandHandler,
        ICommandHandler<RateAuthorCommand> rateAuthorCommandHandler,
        IUserService userService,
        ILogger<RatingController> logger
        )
    {
        this.rateBookCommandHandler = rateBookCommandHandler;
        this.rateAuthorCommandHandler = rateAuthorCommandHandler;
        this.userService = userService;
        this.logger = logger;
    }
    
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RateItem([FromForm] int currentPage, [FromForm] string itemType, [FromForm] int itemId, [FromForm] int score)
    {
        var user = await userService.GetCurrentUserAsync();
        
        if (itemType == RatingItemType.Book.GetName())
        {
            await RateBookAsync(itemId, user.Id, score);
            
            return RedirectToAction("BooksList", "Books", new
            {
                CurrentPage = currentPage,
                RowCount = 10,
                IsDeleted = false
            });
        }

        await RateAuthorAsync(itemId, user.Id, score);
        
        return RedirectToAction("AuthorList", "Authors", new
        {
            CurrentPage = currentPage,
            RowCount = 10,
            IsDeleted = false
        });
    }
    
    private async Task RateBookAsync(int bookId, Guid userId, int score)
    {
        logger.LogInformation($"User with ID: {userId} is rating book with ID: {bookId} with score: {score}");
        var command = new RateBookCommand(bookId, userId, score);
        await rateBookCommandHandler.Handler(command);
    }

    private async Task RateAuthorAsync(int authorId, Guid userId, int score)
    {
        logger.LogInformation($"User with ID: {userId} is rating author with ID: {authorId} with score: {score}");
        var command = new RateAuthorCommand(authorId, userId, score);
        await rateAuthorCommandHandler.Handler(command);
    }
}