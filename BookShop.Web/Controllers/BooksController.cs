using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Infrastructure.Filters;
using BookShop.Infrastructure.Services.PolicyRole;
using BookShop.Shared.Enums;
using BookShop.Shared.Pagination.Abstractions;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;
[ValidationExceptionFilter("Book")]
public class BooksController: Controller
{

    private readonly ICommandHandler<CreateBookCommand> createBookCommandHandler;
    private readonly IQueryHandler<GetBookListQuery, IPagedResult<BookModel>> getBookListQueryHandler;
    private readonly IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>> getAuthorsListQueryHandler;
    private readonly IQueryHandler<GetBookQuery, BookModel> getBookQueryHandler;
    private readonly ICommandHandler<SoftDeleteBookCommand> softDeleteBookCommandHandler;
    private readonly ICommandHandler<UpdateBookCommand> updateBookCommandHandler;
    private readonly IPolicyRoleService policyRoleService;

    public BooksController(
        ICommandHandler<CreateBookCommand> createBookCommandHandler, 
        IQueryHandler<GetBookListQuery, IPagedResult<BookModel>> getBookListQueryHandler, 
        IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>> getAuthorsListQueryHandler,
        ICommandHandler<SoftDeleteBookCommand> softDeleteBookCommandHandler,
        IQueryHandler<GetBookQuery, BookModel> getBookQueryHandler,
        ICommandHandler<UpdateBookCommand> updateBookCommandHandler,
        IPolicyRoleService policyRoleService
        )
    {
        this.createBookCommandHandler = createBookCommandHandler;
        this.getBookListQueryHandler = getBookListQueryHandler;
        this.getAuthorsListQueryHandler = getAuthorsListQueryHandler;
        this.softDeleteBookCommandHandler = softDeleteBookCommandHandler;
        this.getBookQueryHandler = getBookQueryHandler;
        this.updateBookCommandHandler = updateBookCommandHandler;
        this.policyRoleService = policyRoleService;
    }


    [HttpGet]
    [Authorize(Policy = nameof(Policies.AdminAndManager))]
    public async Task<IActionResult> BookForm(int? id)
    {


        var modelT = new BookDetailsViewModel();
        if (id is null)
        {
            
            modelT.Book = new BookModel();
            modelT = await PopulateAuthorsAsync(modelT);
            return View("CreateBook", modelT);
        }

        var book = await getBookQueryHandler.Handler(new GetBookQuery(id.Value));
        
        if (book == null)
        {
            return NotFound();
        }

        modelT = new BookDetailsViewModel()
        {
            Book = book,
            Authors = null
        };
        modelT = await PopulateAuthorsAsync(modelT);
        return View("CreateBook", modelT);

    }
    
    
    [HttpPost]
    [Authorize(Policy = nameof(Policies.AdminAndManager))]
    public async Task<IActionResult> CreateBook([FromForm] BookDetailsViewModel model)
    {
        
        model = await PopulateAuthorsAsync(model);

        if (!ModelState.IsValid) return View(model);
        
        HttpContext.Items["CurrentModel"] = model;
        
        await createBookCommandHandler.Handler(new CreateBookCommand(model.Book.AuthorId, model.Book.Title, model.Book.Description, model.Book.ReleaseDate));
        
        return RedirectToAction("BooksList");
    }
    
    
    [HttpPost]
    [Authorize(Policy = nameof(Policies.AdminAndManager))]
    public async Task<IActionResult> EditBook([FromForm] BookDetailsViewModel model)
    {
        model = await PopulateAuthorsAsync(model);
        
        if (!ModelState.IsValid) return View("CreateBook", model);

        HttpContext.Items["CurrentModel"] = model;
        
        await updateBookCommandHandler.Handler(new UpdateBookCommand(model.Book.Id, model.Book.AuthorId, model.Book.Title, model.Book.Description, model.Book.ReleaseDate));
        
        return RedirectToAction("BooksList");
    }

    [Authorize]
    public async Task<IActionResult> BooksList([FromQuery] PageBookQueryModel model)
    {
        
        var isInAdminAndManagerPolicy = await policyRoleService.IsUserInRoleForPolicyAsync(Policies.AdminAndManager, Roles.Admin);
        var shouldShowDeleted = isInAdminAndManagerPolicy && model.IsDeleted;
        
        //TODO: Make to change IsDelete just for admins
        if (model.CurrentPage == 0 || model.RowCount == 0)
        {
            return RedirectToAction("BooksList", new
            {
                CurrentPage = 1,
                RowCount = 10,
                SortDirection = model.SortDirection,
                SearchByBookTitle = model.SearchByBookTitle,
                SearchByAuthorName = model.SearchByAuthorName,
                IsDeleted = shouldShowDeleted
            });
        }

        return View(await getBookListQueryHandler.Handler(new GetBookListQuery(model.CurrentPage, model.RowCount, model.SortDirection, model.SearchByBookTitle, model.SearchByAuthorName, shouldShowDeleted)));
    }
    
    [HttpPost]
    [Authorize(Policy = nameof(Policies.AdminAndManager))]
    public async Task<IActionResult>  RemoveBook([FromForm] int bookId)
    {
        await softDeleteBookCommandHandler.Handler(new SoftDeleteBookCommand(bookId));
        return RedirectToAction("BooksList");
    }


    private async Task<BookDetailsViewModel> PopulateAuthorsAsync(BookDetailsViewModel vm)
    {
        vm.Authors = (await getAuthorsListQueryHandler
            .Handler(new GetAuthorListQuery(1, int.MaxValue)))
            .Items;
        return vm;
    }
}