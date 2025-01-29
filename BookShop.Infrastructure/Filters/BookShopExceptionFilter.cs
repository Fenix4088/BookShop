using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace BookShop.Infrastructure.Filters;

public class BookShopExceptionFilter : IExceptionFilter
{

    private readonly ILogger<BookShopExceptionFilter> _logger;

    public BookShopExceptionFilter(ILogger<BookShopExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BookShopException shopException)
        {
            _logger.LogError("BookShopException: {Message}", shopException.Message);

            if (context.RouteData.Values["controller"] != null && context.RouteData.Values["action"] != null)
            {
                context.ModelState.AddModelError("", shopException.Message);
                context.Result = new ViewResult
                {
                    ViewName = HandleCommonViewNames(context.RouteData.Values["action"]?.ToString()),
                    ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
                };
            }
            else
            {
                context.Result = new RedirectToActionResult("Error", "Home", new { message = shopException.Message });
            }
            
            context.ExceptionHandled = true;
        }

    }

    private string HandleCommonViewNames(string viewName)
    {
        return viewName switch
        {
            "EditAuthor" => "CreateAuthor",
            _ => viewName
        };
    }
}