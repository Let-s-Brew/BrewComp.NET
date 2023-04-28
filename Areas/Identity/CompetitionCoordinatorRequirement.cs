using Microsoft.AspNetCore.Authorization;

namespace BrewComp.Identity;

public class CompetitionCoordinatorRequirement : IAuthorizationRequirement
{
    public Guid CompetitionId { get; init; }

    public CompetitionCoordinatorRequirement(Guid competitionId)
    {
        CompetitionId = competitionId;
    }
}
