using System;
using System.ComponentModel;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Web;

public static class Extensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
        });

        services.AddIdentity<BookShopUser, BookShopRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
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