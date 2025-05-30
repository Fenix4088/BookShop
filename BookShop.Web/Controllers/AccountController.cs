using System;
using System.Threading.Tasks;
using BookShop.Application.Enums;
using BookShop.Infrastructure.Identity;
using BookShop.Shared;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookShop.Web.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<BookShopUser> userManager;
    private readonly SignInManager<BookShopUser> signInManager;
    private readonly RoleManager<BookShopRole> roleManager;
    private readonly IEmailSender emailSender;

    public AccountController(
        UserManager<BookShopUser> userManager,
        SignInManager<BookShopUser> signInManager,
        RoleManager<BookShopRole> roleManager,
        IEmailSender emailSender)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
        this.emailSender = emailSender;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            ModelState.AddModelError("", "Invalid login");
            return View();
        }
        
        if(user.EmailConfirmed == false)
        {
            ModelState.AddModelError("", "Email is not confirmed");
            return View();
        }

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

        return View();

    }

    [HttpGet]
    public IActionResult Register() => View();
    
    [HttpGet]
    public IActionResult EmailConfirmation(EmailConfirmationModel model) => View(model);

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
            await SendConfirmationEmail(user);
            await userManager.AddToRoleAsync(user, Roles.User.GetName());
            
            return RedirectToAction("EmailConfirmation", "Account", new EmailConfirmationModel()
            {
                Email = email
            });
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

    private async Task SendConfirmationEmail(BookShopUser user)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(nameof(EmailConfirmation), "Account", new { token, email = user.Email }, Request.Scheme);
        var message = $"Please confirm your email by clicking this link: <a href=\"{confirmationLink}\">Confirm Email</a>";
        await emailSender.SendEmailAsync(user.Email, "Confirm your email", message);
    }
}