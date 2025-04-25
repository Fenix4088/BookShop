using System;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Models.Queries;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;

public class BooksController: Controller
{

    private readonly ICommandHandler<CreateBookCommand> createBookCommandHandler;
    private readonly IQueryHandler<GetBookListQuery, IPagedResult<BookModel>> getBookListQueryHandler;
    private readonly IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>> getAuthorsListQueryHandler;


    public BooksController(
        ICommandHandler<CreateBookCommand> createBookCommandHandler, 
        IQueryHandler<GetBookListQuery, IPagedResult<BookModel>> getBookListQueryHandler, 
        IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>> getAuthorsListQueryHandler)
    {
        this.createBookCommandHandler = createBookCommandHandler;
        this.getBookListQueryHandler = getBookListQueryHandler;
        this.getAuthorsListQueryHandler = getAuthorsListQueryHandler;
    }


    [HttpGet]
    public async Task<IActionResult> BookForm()
    {
        var model = new BookDetailsViewModel {
            Book    = new BookModel(),
            Authors = null // will be filled next
        };
        model = await PopulateAuthorsAsync(model);
        return View("CreateBook", model);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> CreateBook([FromForm] BookDetailsViewModel model)
    {
        
        model = await PopulateAuthorsAsync(model);

        if (!ModelState.IsValid) return View(model);
        
        await createBookCommandHandler.Handler(new CreateBookCommand(model.Book.AuthorId, model.Book.Title, model.Book.Description, model.Book.ReleaseDate, model.Book.CoverImgUrl));
        
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


    private async Task<BookDetailsViewModel> PopulateAuthorsAsync(BookDetailsViewModel vm)
    {
        vm.Authors = (await getAuthorsListQueryHandler
            .Handler(new GetAuthorListQuery(1, int.MaxValue)))
            .Items;
        return vm;
    }
}