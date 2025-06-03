using System.Threading.Tasks;
using BookShop.Application.Enums;
using BookShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Infrastructure.Services.User;

public interface IUserService
{
    
    Task<BookShopUser> GetCurrentUserAsync();
    Task<bool> IsUserAuthenticatedAsync();
    Task<bool> IsUserInRoleAsync(Roles role);
    Task<BookShopUser> GetCurrentUserByEmail(string email);
}