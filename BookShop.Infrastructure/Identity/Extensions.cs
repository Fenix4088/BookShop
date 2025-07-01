using BookShop.Web.Models;

namespace BookShop.Infrastructure.Identity;

public static class Extensions
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