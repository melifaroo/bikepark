using System.Threading.Tasks;
using Bikepark.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Bikepark.Authorization
{
    public class BikeparkAdministratorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Item>
    {
        protected override Task HandleRequirementAsync( AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Item resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Administrators can do anything.
            if (context.User.IsInRole(Constants.AdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}