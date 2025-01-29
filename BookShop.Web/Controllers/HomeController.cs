using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Error(string message)
    {
        ViewData["ErrorMessage"] = message ?? "An unexpected error occurred.";
        return View("Error");
    }
}