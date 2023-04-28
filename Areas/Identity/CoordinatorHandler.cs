using BrewComp.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BrewComp.Areas.Identity;

public class CoordinatorHandler : AuthorizationHandler<CompetitionCoordinatorRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CompetitionCoordinatorRequirement requirement)
    {
        var compClaim = context.User.FindFirst(c => 
                c.Type.Equals("Coordinator", StringComparison.OrdinalIgnoreCase)
                    && c.Value.Equals(requirement.CompetitionId)
            );

        if (compClaim is null )
        {
            return Task.CompletedTask;
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
