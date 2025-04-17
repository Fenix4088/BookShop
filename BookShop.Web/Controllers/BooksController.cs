using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Infrastructure.Handlers;
using BookShop.Models.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;

public class BooksController: Controller
{

    private readonly ICommandHandler<CreateBookCommand> createBookCommandHandler;
    private readonly IQueryHandler<GetBookListQuery, IPagedResult<BookModel>> getBookListQueryHandler;


    public BooksController(ICommandHandler<CreateBookCommand> createBookCommandHandler, IQueryHandler<GetBookListQuery, IPagedResult<BookModel>> getBookListQueryHandler)
    {
        this.createBookCommandHandler = createBookCommandHandler;
        this.getBookListQueryHandler = getBookListQueryHandler;
    }


    [HttpGet]
    public async Task<IActionResult> BookForm()
    {
        return View("CreateBook", new BookModel());
    }
    
    
    [HttpPost]
    public async Task<IActionResult> CreateBook([FromForm] BookModel model)
    {

        if (!ModelState.IsValid) return View(model);
        
        await createBookCommandHandler.Handler(new CreateBookCommand(model.AuthorId, model.Title, model.Description, model.ReleaseDate, model.CoverImgUrl));
        
        return RedirectToAction("BooksList");
    }


    public async Task<IActionResult> BooksList([FromQuery] PagedQueryModel model)
    {
        
        if (model.CurrentPage == 0 || model.RowCount == 0)
        {
            return RedirectToAction("BooksList", new { CurrentPage = 1, RowCount = 10 });
        }

        return View(await getBookListQueryHandler.Handler(new GetBookListQuery(model.CurrentPage, model.RowCount)));
    }
}