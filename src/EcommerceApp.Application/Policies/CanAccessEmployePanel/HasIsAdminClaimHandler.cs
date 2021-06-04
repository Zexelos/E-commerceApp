using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceApp.Application.Policies
{
    public class HasIsAdminClaimHandler : AuthorizationHandler<CanAccessEmployeePanelRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanAccessEmployeePanelRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "IsAdmin" && c.Value == "True"))
            {
                return Task.CompletedTask;
            }
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
