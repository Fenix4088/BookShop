using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BookShop.Infrastructure.Middlewares;

internal sealed class ExceptionsMiddleware: IMiddleware
{
    private readonly ILogger<ExceptionsMiddleware> _logger;

    public ExceptionsMiddleware(ILogger<ExceptionsMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        //! Handle just Unexpected errors
        string redirectUrl = "/Home/Error?message=An unexpected error occurred.";
        context.Response.Redirect(redirectUrl);
    }
}