using Microsoft.AspNetCore.Mvc;

namespace BookShop.Infrastructure.Filters;

public class ValidationExceptionFilterAttribute 
    : TypeFilterAttribute
{
    public ValidationExceptionFilterAttribute(string? prefix = "")
        : base(typeof(ValidationExceptionFilter))
    {
        Arguments = new object[] { prefix };
    }
}