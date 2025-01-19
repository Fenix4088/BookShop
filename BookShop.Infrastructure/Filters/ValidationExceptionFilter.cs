using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace BookShop.Infrastructure.Filters;

public class ValidationExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ValidationExceptionFilter> _logger;

    public ValidationExceptionFilter(ILogger<ValidationExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            foreach (var error in validationException.Errors)
            {
                context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            _logger.LogError("Validation failed: {Errors}", validationException.Errors);
            context.Result = new ViewResult
            {
                ViewName = context.RouteData.Values["action"]?.ToString(),
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
            };

            context.ExceptionHandled = true;
        }
    }
}