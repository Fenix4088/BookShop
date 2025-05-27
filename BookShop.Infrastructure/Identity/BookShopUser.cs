using System;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Infrastructure.Identity;

public class BookShopUser: IdentityUser<Guid>
{
}