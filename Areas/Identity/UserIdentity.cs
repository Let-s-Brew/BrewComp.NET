﻿using BrewComp.Data;
using Microsoft.AspNetCore.Identity;
using BrewCode.AddressTools.Models;

namespace BrewComp.Identity;

public class UserIdentity : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public CivicAddress? Address { get; set; }
    public IEnumerable<Competition> Competitions { get; set; } = new List<Competition>();
    public HomebrewClub? Club { get; set; }

    public UserIdentity(string firstName, string lastName, IEnumerable<Competition> competitions, CivicAddress? address = null, HomebrewClub? club = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Competitions = competitions;
        Club = club;
    }

    public UserIdentity(string firstName, string lastName)
      :this(firstName, lastName, new List<Competition>()) { }
}
