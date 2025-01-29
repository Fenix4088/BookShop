using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Models.Queries;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;
public class AuthorsController : Controller
{
    private readonly ICommandHandler<CreateAuthorCommand> createAuthorCommandHandler;
    private readonly ICommandHandler<RemoveAuthorCommand> removeAuthorCommandHandler;
    private readonly IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>> getAuthorListQueryHandler;
    private readonly IQueryHandler<GetAuthorQuery, AuthorModel> getAuthorQueryHandler;
    private readonly ICommandHandler<UpdateAuthorCommand> updateAuthorCommandHandler;

    public AuthorsController(
        ICommandHandler<CreateAuthorCommand> createAuthorCommandHandler, 
        ICommandHandler<RemoveAuthorCommand> removeAuthorCommandHandler,
        IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>> getAuthorListQueryHandler,
        IQueryHandler<GetAuthorQuery, AuthorModel> getAuthorQueryHandler,
        ICommandHandler<UpdateAuthorCommand> updateAuthorCommandHandler
        )
        
    {
        this.createAuthorCommandHandler = createAuthorCommandHandler;
        this.getAuthorListQueryHandler = getAuthorListQueryHandler;
        this.removeAuthorCommandHandler = removeAuthorCommandHandler;
        this.getAuthorQueryHandler = getAuthorQueryHandler;
        this.updateAuthorCommandHandler = updateAuthorCommandHandler;
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }

    [HttpGet]
    public IActionResult CreateAuthor()
    {
        return View(new AuthorModel());
    }

    [HttpGet]
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
    public async Task<IActionResult>  RemoveAuthor([FromForm] int authorId)
    {
        await removeAuthorCommandHandler.Handler(new RemoveAuthorCommand(authorId));
        return RedirectToAction("AuthorList");
    }
    
    [HttpPost]
    public async Task<IActionResult> EditAuthor([FromForm] AuthorModel model)
    {
        if (!ModelState.IsValid) return View("CreateAuthor", model);

        HttpContext.Items["CurrentModel"] = model;
        
        await updateAuthorCommandHandler.Handler(new UpdateAuthorCommand(model.Id, model.Name, model.Surname));
        
        return RedirectToAction("AuthorList");
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuthor([FromForm] AuthorModel model)
    {

        if (!ModelState.IsValid) return View(model);
        
        await createAuthorCommandHandler.Handler(new CreateAuthorCommand(model.Name, model.Surname));
        
        return RedirectToAction("AuthorList");
    }


    public async Task<IActionResult> AuthorList([FromQuery] PagedQueryModel model)
    {
        if (model.CurrentPage == 0 || model.RowCount == 0)
        {
            return RedirectToAction("AuthorList", new { CurrentPage = 1, RowCount = 10 });
        }

        return View(await getAuthorListQueryHandler.Handler(new GetAuthorListQuery(model.CurrentPage, model.RowCount)));
    }
}