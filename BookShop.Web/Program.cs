using BookShop.Application;
using BookShop.Infrastructure;
using BookShop.Infrastructure.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddControllersWithViews(options =>
    {
        // options.Filters.Add<ValidationExceptionFilter>();
        options.Filters.Add<BookShopExceptionFilter>();
    });

var app = builder.Build();

app.UseInfrastructure();
app.Run();        