using BrewCode.AddressTools.Models;
using BrewComp.Data;
using Microsoft.AspNetCore.Identity;

namespace BrewComp.Identity;

public class BrewCompUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public CivicAddress? Address { get; set; }
    public ICollection<Competition> Competitions { get; set; } = new List<Competition>();
    public HomebrewClub? Club { get; set; }
    public ICollection<CompetitionEntry> Entries { get; set; } = new List<CompetitionEntry>();

    public BrewCompUser(string firstName, string lastName, ICollection<Competition> competitions, ICollection<CompetitionEntry> entries, CivicAddress? address = null, HomebrewClub? club = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Competitions = competitions;
        Club = club;
        Entries = entries;
    }

    public BrewCompUser(string firstName, string lastName)
      : this(firstName, lastName, new List<Competition>(), new List<CompetitionEntry>()) { }
}
