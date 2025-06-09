using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Application.Enums;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Infrastructure.Services.PolicyRole;
using BookShop.Infrastructure.Services.User;
using BookShop.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;

public class RatingController : Controller
{
    
    private readonly ICommandHandler<RateBookCommand> rateBookCommandHandler;
    private readonly IUserService userService;
    private readonly IQueryHandler<GetBookListQuery, IPagedResult<BookModel>> getBookListQueryHandler;
    private readonly IPolicyRoleService policyRoleService;
    
    
    
    public RatingController(
        ICommandHandler<RateBookCommand> rateBookCommandHandler, 
        IUserService userService,
        IQueryHandler<GetBookListQuery, IPagedResult<BookModel>> getBookListQueryHandler,
        IPolicyRoleService policyRoleService
        )
    {
        this.rateBookCommandHandler = rateBookCommandHandler;
        this.userService = userService;
        this.getBookListQueryHandler = getBookListQueryHandler;
        this.policyRoleService = policyRoleService;
    }
    
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RateItem([FromForm] int currentPage, [FromForm] string itemType, [FromForm] int itemId, [FromForm] int score)
    {
        var user = await userService.GetCurrentUserAsync();
        
        var isInAdminAndManagerPolicy = await policyRoleService.IsUserInRoleForPolicyAsync(Policies.AdminAndManager, Roles.Admin);

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