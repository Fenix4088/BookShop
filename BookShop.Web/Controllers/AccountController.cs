using System;
using System.Threading.Tasks;
using BookShop.Application.Enums;
using BookShop.Infrastructure.Identity;
using BookShop.Infrastructure.Services.User;
using BookShop.Shared;
using BookShop.Web.Attributes;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookShop.Web.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<BookShopUser> userManager;
    private readonly SignInManager<BookShopUser> signInManager;
    private readonly IEmailSender emailSender;
    private readonly IUserService userService;

    public AccountController(
        UserManager<BookShopUser> userManager,
        SignInManager<BookShopUser> signInManager,
        IEmailSender emailSender, IUserService userService)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.emailSender = emailSender;
        this.userService = userService;
    }

    [HttpGet]
    [DenyAuthenticated]
    public IActionResult Login() => View();

    [HttpPost]
    [DenyAuthenticated]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await userService.GetCurrentUserByEmail(email);

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
    [DenyAuthenticated]
    public IActionResult Register() => View();
    
    [HttpGet]
    [DenyAuthenticated]
    public IActionResult EmailConfirmationWarning(EmailConfirmationModel model) => View(model);

    [HttpGet]
    [DenyAuthenticated]
    public async Task<IActionResult> EmailConfirmationSuccess(string token, string email)
    {
        var user = await userService.GetCurrentUserByEmail(email);

        if (user is null)
        {
            return View("Error");
        }

        var result = await userManager.ConfirmEmailAsync(user, token);
        return View(result.Succeeded ? "EmailConfirmationSuccess" : "Error");
    }

    [HttpPost]
    [DenyAuthenticated]
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
            
            return RedirectToAction("EmailConfirmationWarning", "Account", new EmailConfirmationModel()
            {
                Email = email
            });
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View();
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
    
    [DenyAuthenticated]
    public IActionResult AccessDenied() => View();

    [DenyAuthenticated]
    private async Task SendConfirmationEmail(BookShopUser user)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(nameof(EmailConfirmationSuccess), "Account", new { token, email = user.Email }, Request.Scheme);
        var message = $"Please confirm your email by clicking this link: <a href=\"{confirmationLink}\">Confirm Email</a>";
        await emailSender.SendEmailAsync(user.Email, "Confirm your email", message);
    }
}