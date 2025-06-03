using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Application.Enums;
using BookShop.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BookShop.Infrastructure.Services.PolicyRole;

public class PolicyRoleService : IPolicyRoleService
{
    private readonly IAuthorizationPolicyProvider _policyProvider;

    public PolicyRoleService(IAuthorizationPolicyProvider policyProvider)
    {
        _policyProvider = policyProvider;
    }

    public async Task<IEnumerable<string>> GetRolesForPolicyAsync(Policies policies)
    {
        var policyName = policies.GetName();
        var policy = await _policyProvider.GetPolicyAsync(policyName);
        if (policy == null)
        {
            return Enumerable.Empty<string>();
        }

        var roleRequirements = policy.Requirements
            .OfType<RolesAuthorizationRequirement>()
            .SelectMany(r => r.AllowedRoles);

        return roleRequirements;
    }
    
    public async Task<bool> IsUserInRoleForPolicyAsync(Policies policies, Roles role)
    {
        var roles = await GetRolesForPolicyAsync(policies);
        return roles.Contains(role.GetName());
    }
}