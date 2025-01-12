using BookShop.Domain.Validators;
using BookShop.Infrastructure;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Handlers.Commands;
using BookShop.Infrastructure.Handlers.Queries;
using BookShop.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateAuthorValidation>());

// builder.Services.AddDbContext<ShopDbContext>(options =>
//     options.UseSqlServer(
//         builder.Configuration.GetConnectionString("BookShop")));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddTransient<AuthorRepository>();
builder.Services.AddTransient<CreateAuthorCommandHandler>();
builder.Services.AddTransient<GetAuthorListQueryHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Authors}/{action=AuthorList}/{id?}");

app.Run();        