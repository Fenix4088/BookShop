using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Application.Models;
using BookShop.Infrastructure.Handlers;
using BookShop.Models.Queries;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ICommandHandler<CreateAuthorCommand> createAuthorCommandHandler;
        private readonly IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>> getAuthorListQueryHandler;

        public AuthorsController(ICommandHandler<CreateAuthorCommand> createAuthorCommandHandler,
            IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>> getAuthorListQueryHandler)
        {
            this.createAuthorCommandHandler = createAuthorCommandHandler;
            this.getAuthorListQueryHandler = getAuthorListQueryHandler;
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

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromForm] AuthorModel model)
        {

            if (!ModelState.IsValid) return View(model);
            try
            {
                await createAuthorCommandHandler.Handler(new CreateAuthorCommand(model.Name, model.Surname));
                return RedirectToAction("AuthorList");
            }
            catch (ValidationException e)
            {
                foreach (var error in e.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AuthorList([FromQuery] PagedQueryModel model)
        {
            return View(await getAuthorListQueryHandler.Handler(new GetAuthorListQuery(model.CurrentPage, model.RowCount)));
        }
    }
}