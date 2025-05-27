using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Infrastructure.Filters;
using BookShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;
[ValidationExceptionFilter]
public class AuthorsController : Controller
{
    private readonly ICommandHandler<CreateAuthorCommand> createAuthorCommandHandler;
    private readonly ICommandHandler<SoftDeleteAuthorCommand> softDeleteAuthorCommandHandler;
    private readonly IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>> getAuthorListQueryHandler;
    private readonly IQueryHandler<GetAuthorQuery, AuthorModel> getAuthorQueryHandler;
    private readonly ICommandHandler<UpdateAuthorCommand> updateAuthorCommandHandler;    
    private readonly SignInManager<BookShopUser> signInManager;


    public AuthorsController(
        ICommandHandler<CreateAuthorCommand> createAuthorCommandHandler, 
        ICommandHandler<SoftDeleteAuthorCommand> softDeleteAuthorCommandHandler,
        IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>> getAuthorListQueryHandler,
        IQueryHandler<GetAuthorQuery, AuthorModel> getAuthorQueryHandler,
        ICommandHandler<UpdateAuthorCommand> updateAuthorCommandHandler,
        SignInManager<BookShopUser> signInManager
        )
        
    {
        this.createAuthorCommandHandler = createAuthorCommandHandler;
        this.getAuthorListQueryHandler = getAuthorListQueryHandler;
        this.softDeleteAuthorCommandHandler = softDeleteAuthorCommandHandler;
        this.getAuthorQueryHandler = getAuthorQueryHandler;
        this.updateAuthorCommandHandler = updateAuthorCommandHandler;
        this.signInManager = signInManager;
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateAuthor()
    {
        return View(new AuthorModel());
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AuthorForm(int? id)
    {
        if (id == null)
        {
            return View("CreateAuthor", new AuthorModel());
        }

        var author = await getAuthorQueryHandler.Handler(new GetAuthorQuery(id));

        if (author == null)
        {
            return NotFound();
        }

        return View("CreateAuthor", author);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult>  RemoveAuthor([FromForm] int authorId)
    {
        await softDeleteAuthorCommandHandler.Handler(new SoftDeleteAuthorCommand(authorId));
        return RedirectToAction("AuthorList");
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditAuthor([FromForm] AuthorModel model)
    {
        if (!ModelState.IsValid) return View("CreateAuthor", model);

        HttpContext.Items["CurrentModel"] = model;
        
        await updateAuthorCommandHandler.Handler(new UpdateAuthorCommand(model.Id, model.Name, model.Surname));
        
        return RedirectToAction("AuthorList");
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAuthor([FromForm] AuthorModel model)
    {

        if (!ModelState.IsValid) return View(model);
        
        await createAuthorCommandHandler.Handler(new CreateAuthorCommand(model.Name, model.Surname));
        
        return RedirectToAction("AuthorList");
    }

    [Authorize]
    public async Task<IActionResult> AuthorList([FromQuery] PageAuthorQueryModel model)
    {
        //TODO: Make to change IsDelete just for admins
        if (model.CurrentPage == 0 || model.RowCount == 0)
        {
            return RedirectToAction("AuthorList", new
            {
                CurrentPage = 1, 
                RowCount = 10,  
                SortDirection = model.SortDirection, 
                SearchByNameAndSurname = model.SearchByNameAndSurname, 
                IsDeleted = model.IsDeleted
            });
        }

        return View(await getAuthorListQueryHandler.Handler(new GetAuthorListQuery(model.CurrentPage, model.RowCount, model.SortDirection, model.SearchByNameAndSurname, model.IsDeleted)));
    }
}