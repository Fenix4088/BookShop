using System.Threading.Tasks;
using BookShop.Infrastructure.Identity;
using BookShop.Shared.Enums;

namespace BookShop.Infrastructure.Services.User;

public interface IUserService
{
    
    Task<BookShopUser> GetCurrentUserAsync();
    Task<bool> IsUserAuthenticatedAsync();
    Task<bool> IsUserInRoleAsync(Roles role);
    Task<BookShopUser> GetCurrentUserByEmail(string email);
}