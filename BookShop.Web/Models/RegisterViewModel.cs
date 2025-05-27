using System.ComponentModel.DataAnnotations;

namespace BookShop.Web.Models;

public class RegisterViewModel
{
    [Microsoft.Build.Framework.Required, EmailAddress]
    public string Email { get; set; }

    [Microsoft.Build.Framework.Required, DataType(DataType.Password)]
    public string Password { get; set; }

    [Microsoft.Build.Framework.Required, Compare("Password")]
    public string ConfirmPassword { get; set; }
}