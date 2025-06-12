using System.ComponentModel;

namespace BookShop.Shared.Enums;

public enum Roles
{
    [Description("Administrator")]
    Admin = 1,
    [Description("Regular User")]
    User,
    [Description("Manager")]
    Manager,
}