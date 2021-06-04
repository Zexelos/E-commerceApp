using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceApp.Application.Policies
{
    public class HasIsEmployeeClaimHandler : AuthorizationHandler<CanAccessEmployeePanelRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanAccessEmployeePanelRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "IsEmployee" && c.Value == "True"))
            {
                return Task.CompletedTask;
            }
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
