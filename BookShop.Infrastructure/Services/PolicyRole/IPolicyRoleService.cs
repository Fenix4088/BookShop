using System.Collections.Generic;
using System.Threading.Tasks;
using BookShop.Shared.Enums;

namespace BookShop.Infrastructure.Services.PolicyRole;

public interface IPolicyRoleService
{
    Task<IEnumerable<string>> GetRolesForPolicyAsync(Policies policies);
    Task<bool> IsUserInRoleForPolicyAsync(Policies policies, Roles role);
}