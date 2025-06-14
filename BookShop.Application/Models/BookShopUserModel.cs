using System;

namespace BookShop.Web.Models;

public class BookShopUserModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
}