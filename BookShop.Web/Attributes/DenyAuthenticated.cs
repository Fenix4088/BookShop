using System;
using BookShop.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookShop.Web.Attributes;

internal sealed class DenyAuthenticated : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        base.OnActionExecuted(context);
        if (context.HttpContext.User.Identity?.IsAuthenticated == true)
        {
            context.Result = new RedirectToActionResult("AuthorList", "Authors", new
            {
                CurrentPage = 1, 
                RowCount = 10,  
                SortDirection = SortDirection.Descending, 
                SearchByNameAndSurname = String.Empty, 
                IsDeleted = false
            });
        }
    }
}