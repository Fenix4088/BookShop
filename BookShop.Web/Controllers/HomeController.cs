using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}