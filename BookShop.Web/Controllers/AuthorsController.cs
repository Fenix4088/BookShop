﻿using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Commands;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Infrastructure.Filters;
using BookShop.Infrastructure.Services.PolicyRole;
using BookShop.Shared.Enums;
using BookShop.Shared.Pagination.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookShop.Web.Controllers;
[ValidationExceptionFilter]
public class AuthorsController : Controller
{
    private readonly ICommandHandler<CreateAuthorCommand> createAuthorCommandHandler;
    private readonly ICommandHandler<SoftDeleteAuthorCommand> softDeleteAuthorCommandHandler;
    private readonly IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>> getAuthorListQueryHandler;
    private readonly IQueryHandler<GetAuthorQuery, AuthorModel> getAuthorQueryHandler;
    private readonly ICommandHandler<UpdateAuthorCommand> updateAuthorCommandHandler;    
    private readonly IPolicyRoleService policyRoleService;
    private readonly ILogger<AuthorsController> logger;


    public AuthorsController(
        ICommandHandler<CreateAuthorCommand> createAuthorCommandHandler, 
        ICommandHandler<SoftDeleteAuthorCommand> softDeleteAuthorCommandHandler,
        IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>> getAuthorListQueryHandler,
        IQueryHandler<GetAuthorQuery, AuthorModel> getAuthorQueryHandler,
        ICommandHandler<UpdateAuthorCommand> updateAuthorCommandHandler,
        IPolicyRoleService policyRoleService,
        ILogger<AuthorsController> logger
        )
        
    {
        this.createAuthorCommandHandler = createAuthorCommandHandler;
        this.getAuthorListQueryHandler = getAuthorListQueryHandler;
        this.softDeleteAuthorCommandHandler = softDeleteAuthorCommandHandler;
        this.getAuthorQueryHandler = getAuthorQueryHandler;
        this.updateAuthorCommandHandler = updateAuthorCommandHandler;
        this.policyRoleService = policyRoleService;
        this.logger = logger;
    }
    

    [HttpGet]
    [Authorize(Policy = nameof(Policies.AdminAndManager))]
    public IActionResult CreateAuthor()
    {
        return View(new AuthorModel());
    }

    [HttpGet]
    [Authorize(Policy = nameof(Policies.AdminAndManager))]
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
    [Authorize(Policy = nameof(Policies.AdminAndManager))]
    public async Task<IActionResult>  RemoveAuthor([FromForm] int authorId)
    {
        await softDeleteAuthorCommandHandler.Handler(new SoftDeleteAuthorCommand(authorId));
        return RedirectToAction("AuthorList");
    }
    
    [HttpPost]
    [Authorize(Policy = nameof(Policies.AdminAndManager))]
    public async Task<IActionResult> EditAuthor([FromForm] AuthorModel model)
    {
        if (!ModelState.IsValid) return View("CreateAuthor", model);

        HttpContext.Items["CurrentModel"] = model;
        
        await updateAuthorCommandHandler.Handler(new UpdateAuthorCommand(model.Id, model.Name, model.Surname));
        
        return RedirectToAction("AuthorList");
    }

    [HttpPost]
    [Authorize(Policy = nameof(Policies.AdminAndManager))]
    public async Task<IActionResult> CreateAuthor([FromForm] AuthorModel model)
    {

        if (!ModelState.IsValid) return View(model);
        
        await createAuthorCommandHandler.Handler(new CreateAuthorCommand(model.Name, model.Surname));
        
        return RedirectToAction("AuthorList");
    }

    [Authorize]
    public async Task<IActionResult> AuthorList([FromQuery] PageAuthorQueryModel model)
    {
        var isInAdminAndManagerPolicy = await policyRoleService.IsUserInRoleForPolicyAsync(Policies.AdminAndManager, Roles.Admin);
        
        var shouldShowDeleted = isInAdminAndManagerPolicy && model.IsDeleted;
        
        if (model.CurrentPage == 0 || model.RowCount == 0)
        {
            return RedirectToAction("AuthorList", new
            {
                CurrentPage = 1, 
                RowCount = 10,  
                SortDirection = model.SortDirection, 
                SearchByNameAndSurname = model.SearchByNameAndSurname, 
                IsDeleted = shouldShowDeleted
            });
        }

        return View(await getAuthorListQueryHandler.Handler(new GetAuthorListQuery(model.CurrentPage, model.RowCount, model.SortDirection, model.SearchByNameAndSurname, shouldShowDeleted)));
    }
}