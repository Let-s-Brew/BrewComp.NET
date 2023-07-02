using BrewComp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BrewComp.Pages;

public partial class EditCompetition : ComponentBase
{
    [Parameter]
    public string Id { get; set; } = null!;

    [Inject]
    private BrewCompDbContext DbContext { get; set; } = null!;

    [CascadingParameter]
    private Task<AuthenticationState>? authState { get; set; }

    private bool _loaded = false;
    private bool _authed = false;
    private Competition? _comp = null!;

    protected override async Task OnInitializedAsync()
    {
        if (authState != null)
        {
            var state = await authState;
            var user = state.User;
            if (user != null && user.Identity != null)
            {
                _authed = user.Identity.IsAuthenticated
                    && (user.IsInRole("siteadmin") 
                        || (user.IsInRole("coordinator") && user.HasClaim("CompCoordinator", Id)));
            }
        }

        if(DbContext != null)
        {
            _comp = DbContext.Competitions.Single(c => c.Id.ToString() == Id);
        }
        _loaded = true;
    }
}
