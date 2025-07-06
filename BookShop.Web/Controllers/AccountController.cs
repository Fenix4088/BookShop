using System;
using System.Threading.Tasks;
using BookShop.Application;
using BookShop.Application.Services;
using BookShop.Infrastructure.Identity;
using BookShop.Infrastructure.Services.Cart;
using BookShop.Infrastructure.Services.User;
using BookShop.Shared;
using BookShop.Shared.Enums;
using BookShop.Web.Attributes;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace BookShop.Web.Controllers;
public class AccountController(
    UserManager<BookShopUser> userManager,
    SignInManager<BookShopUser> signInManager,
    IEmailSender emailSender,
    IUserService userService,
    ICartService cartService,
    ILogger<AccountController> logger)
    : Controller
{
    [HttpGet]
    [DenyAuthenticated]
    public IActionResult Login() => View();

    [HttpPost]
    [DenyAuthenticated]
    public async Task<IActionResult> Login(string email, string password)
    {
        logger.LogInformation($"Trying to login user with email: {email}");
        
        var user = await userService.GetCurrentUserByEmail(email);

        if (user is null)
        {
            ModelState.AddModelError("", "Invalid login");
            logger.LogWarning($"Login failed for user with email: {email}. User not found.");
            return View();
        }
        
        if(user.EmailConfirmed == false)
        {
            ModelState.AddModelError("", "Email is not confirmed");
            logger.LogWarning($"Login failed for user with email: {email}. Email not confirmed.");
            return View();
        }

        var result = await signInManager.PasswordSignInAsync(user, password, true, false);
        if (result.Succeeded)
        {
            logger.LogInformation($"User with email: {email} logged in successfully.");
            return RedirectToAction("AuthorList", "Authors", new
            {
                CurrentPage = 1, 
                RowCount = 10,  
                SortDirection = SortDirection.Descending, 
                SearchByNameAndSurname = String.Empty, 
                IsDeleted = false
            });
        }

        logger.LogWarning($"Login failed for user with email: {email}. Invalid password.");
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
        
        if (result.Succeeded)
        {
            logger.LogInformation($"Email confirmed for user with email: {email}");
            await cartService.CreateCartByUserIdAsync(user.Id);
            return View("EmailConfirmationSuccess");
        }

        logger.LogWarning($"Email confirmation failed for user with email: {email}. Errors: {string.Join(", ", result.Errors)}");
        return View("Error");
    }

    [HttpPost]
    [DenyAuthenticated]
    public async Task<IActionResult> Register(string email, string password)
    {
        
        logger.LogInformation($"Trying to register user with email: {email}");
        
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
            
            logger.LogInformation($"User with email: {email} registered successfully.");
            
            return RedirectToAction("EmailConfirmationWarning", "Account", new EmailConfirmationModel()
            {
                Email = email
            });
        }

        logger.LogWarning($"Registration failed for user with email: {email}. Errors: {string.Join(", ", result.Errors)}");
        
        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View();
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        logger.LogInformation("User logged out successfully.");
        return RedirectToAction("Login");
    }
    
    [DenyAuthenticated]
    public IActionResult AccessDenied() => View();

    [DenyAuthenticated]
    private async Task SendConfirmationEmail(BookShopUser user)
    {
        logger.LogInformation($"Sending confirmation email to user with email: {user.Email}");
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(nameof(EmailConfirmationSuccess), "Account", new { token, email = user.Email }, Request.Scheme);
        var message = $"Please confirm your email by clicking this link: <a href=\"{confirmationLink}\">Confirm Email</a>";
        await emailSender.SendEmailAsync(user.Email, "Confirm your email", message);
    }
}