using System.Linq;
using System.Threading.Tasks;
using BookShop.Infrastructure.Identity;
using BookShop.Shared;
using BookShop.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BookShop.Infrastructure.Services.User;

public class UserService(
    UserManager<BookShopUser> userManager,
    SignInManager<BookShopUser> signInManager,
    RoleManager<BookShopRole> roleManager,
    ILogger<UserService> logger)
    : IUserService
{
    private readonly RoleManager<BookShopRole> roleManager = roleManager;

    public async Task<BookShopUser> GetCurrentUserAsync()
    {
        var user = await userManager.GetUserAsync(signInManager.Context?.User);
        logger.LogInformation($"🧟‍♂️ User data were retrieved: {user?.Id}, {user?.UserName}, {user?.Email}");
        return user;
    }

    public async Task<BookShopUser> GetCurrentUserByEmail(string email) => await userManager.FindByEmailAsync(email);

    public async Task<bool> IsUserAuthenticatedAsync()
    {
        var user = await GetCurrentUserAsync();
        return user != null;
    }
    
    public async Task<bool> IsUserInRoleAsync(Roles role)
    {
        
        var user = await GetCurrentUserAsync();
        if (user == null) return false;
        
        return await userManager.IsInRoleAsync(user, role.GetName());
    }
}