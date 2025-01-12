using System;
using BookShop.Models;
using BookShop.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookShop.Application;
using BookShop.Application.Commands;
using BookShop.Application.Handlers.Commands;
using BookShop.Infrastructure.Handlers;
using FluentValidation;

namespace BookShop.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly CreateAuthorCommandHandler createAuthorCommandHandler;
        private readonly GetAuthorListQueryHandler getAuthorListQueryHandler;

        public AuthorsController(CreateAuthorCommandHandler createAuthorCommandHandler,
            GetAuthorListQueryHandler getAuthorListQueryHandler)
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