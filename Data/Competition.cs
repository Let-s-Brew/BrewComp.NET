using BrewComp.Identity;
using NodaTime;

namespace BrewComp.Data;

public class Competition
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public Interval CompetitionDates { get; set; }
    public Interval DropOffDates { get; set; }
    public Interval ShippingDates { get; set; }
    public Interval RegistrationDates { get; set; }
    public Interval EntryRegistrationDates { get; set; }
    public IEnumerable<UserIdentity> Entrants { get; set; } = new List<UserIdentity>();
    public IEnumerable<string> CategoryIds { get; set; } = new List<string>();
    public Dictionary<uint, CompetitionEntry> Entries { get; set; } = new Dictionary<uint, CompetitionEntry>();

}

