
using System;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers;

public class RatingController : Controller
{
    [HttpPost]
    public IActionResult RateItem([FromForm] string itemType, [FromForm] int itemId, [FromForm] int rating)
    {
        return Ok($"Item of type '{itemType}' with ID '{itemId}' rated with {rating} stars.");
    }
}