using System;
using System.Threading.Tasks;
using BookShop.Application.Enums;
using BookShop.Infrastructure.Identity;
using BookShop.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Web.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<BookShopUser> userManager;
    private readonly SignInManager<BookShopUser> signInManager;
    private readonly RoleManager<BookShopRole> roleManager;

    public AccountController(
        UserManager<BookShopUser> userManager,
        SignInManager<BookShopUser> signInManager,
        RoleManager<BookShopRole> roleManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var result = await signInManager.PasswordSignInAsync(user, password, true, false);
            if (result.Succeeded)
            {
                return RedirectToAction("AuthorList", "Authors", new
                {
                    CurrentPage = 1, 
                    RowCount = 10,  
                    SortDirection = SortDirection.Descending, 
                    SearchByNameAndSurname = String.Empty, 
                    IsDeleted = false
                });
            }
        }

        ModelState.AddModelError("", "Invalid login");
        return View();
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password)
    {
        var user = new BookShopUser()
        {
            Email = email,
            UserName = email
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, Roles.User.GetName());
            return RedirectToAction("Login");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied() => View();
}