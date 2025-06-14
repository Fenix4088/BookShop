using BookShop.Web.Models;

namespace BookShop.Infrastructure.Identity;

public static class Extentions
{
    public static BookShopUserModel ToModel(this BookShopUser user)
    {
        return new BookShopUserModel
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName
        };
    }
}