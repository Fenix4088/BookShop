using System.Linq;
using System.Threading.Tasks;
using BookShop.Application.Enums;
using BookShop.Infrastructure.Identity;
using BookShop.Shared;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Infrastructure.Services.User;

public class UserService : IUserService
{
    private readonly UserManager<BookShopUser> userManager;
    private readonly SignInManager<BookShopUser> signInManager;
    private readonly RoleManager<BookShopRole> roleManager;
    
    public UserService(UserManager<BookShopUser> userManager, SignInManager<BookShopUser> signInManager, RoleManager<BookShopRole> roleManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
    }

    public async Task<BookShopUser> GetCurrentUserAsync()
    {
        var user = await userManager.GetUserAsync(signInManager.Context?.User);
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