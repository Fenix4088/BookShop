using System;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace BookShop.Infrastructure.Filters;

//! ValidationExceptionFilter handle all fluent validator errors
public class ValidationExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ValidationExceptionFilter> _logger;
    private readonly string _prefix;

    public ValidationExceptionFilter(ILogger<ValidationExceptionFilter> logger, string prefix)
    {
        _logger = logger;
        _prefix = prefix;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            foreach (var error in validationException.Errors)
            {
                context.ModelState.AddModelError(string.IsNullOrEmpty(_prefix) ? error.PropertyName : $"{_prefix}.{error.PropertyName}", error.ErrorMessage);
            }
            
            _logger.LogError("Validation failed: {Errors}", validationException.Errors);
            
            object? model = context.HttpContext.Items["CurrentModel"];
            
            context.Result = new ViewResult
            {
                ViewName = HandleCommonViewNames(context.RouteData.Values["action"]?.ToString()),
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
                {
                    Model = model
                }
            };

            context.ExceptionHandled = true;
        }
    }
    
    
    private string HandleCommonViewNames(string viewName)
    {
        return viewName switch
        {
            "EditAuthor" => "CreateAuthor",
            "EditBook" => "CreateBook",
            _ => viewName
        };
    }
}