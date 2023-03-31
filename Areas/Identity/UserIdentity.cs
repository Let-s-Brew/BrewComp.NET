using BrewComp.Data;
using Microsoft.AspNetCore.Identity;
using BrewCode.AddressTools.Models;

namespace BrewComp.Identity;

public class UserIdentity : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public CivicAddress Address { get; set; }
    public IEnumerable<Competition> Competitions { get; set; }
    public HomebrewClub? Club { get; set; }
}
