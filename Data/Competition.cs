using BrewCode.AddressTools.Models;
using BrewComp.Identity;
using NodaTime;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewComp.Data;

public class Competition
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public Interval CompetitionDates { get; set; }
    public Interval DropOffDates { get; set; }
    public Interval ShippingDates { get; set; }
    public Interval RegistrationDates { get; set; }
    public Interval EntryRegistrationDates { get; set; }
    public BrewCompUser Host { get; set; } = null!;
    public bool UseClubAsHost { get; set; } = false;
    public List<BrewCompUser> Entrants { get; set; } = new List<BrewCompUser>();
    public List<string> CategoryIds { get; set; } = new List<string>();
    public List<CompetitionEntry> Entries { get; set; } = new List<CompetitionEntry>();
    public List<CivicAddress> DropOffAddresses { get; set; } = new List<CivicAddress>();
    public bool BJCPSanctioned { get; set; } = false;

    // Store coordinators by ID for now, use separate RoleManger for each competition? Create dynamic policies for Coordinator Claims?
    public List<string> CompetitionCoordinators { get; set; } = new List<string>();

}

