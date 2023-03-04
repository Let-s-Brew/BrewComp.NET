using BrewComp.Data;
using Microsoft.AspNetCore.Identity;

namespace BrewComp.Identity;

public class UserIdentity : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public IEnumerable<Competition> Competitions { get; set; }
}
