using BookShop.Web.Models;

namespace BookShop.Application.Users;

public interface IUserRepository
{
    Task<BookShopUserModel> GetByEmailAsync(string email);
    Task<BookShopUserModel> GetByIdAsync(Guid id);
}