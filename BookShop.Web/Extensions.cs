using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Identity;
using BookShop.Shared;
using BookShop.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Web;

public static class Extensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.AdminOnly.GetName(), policy => policy.RequireRole(Roles.Admin.GetName()));
            options.AddPolicy(Policies.AdminAndManager.GetName(), policy => 
                policy.RequireRole(Roles.Admin.GetName(), Roles.Manager.GetName()));
        });

        services.AddIdentity<BookShopUser, BookShopRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.SignIn.RequireConfirmedEmail = true;
        })
            .AddEntityFrameworkStores<ShopDbContext>()
            .AddDefaultTokenProviders();
        
        
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });
        
        return services;
    }
}