using BrewCode.AddressTools.Models;
using BrewComp.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewComp.Data;

public class Competition
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public int? EntryLimit { get; set; } = null!;
    public DateRange CompetitionDates { get; set; }
    public DateRange? DropOffDates { get; set; } = null!;
    public DateRange? ShippingDates { get; set; } = null!;
    public DateRange? RegistrationDates { get; set; } = null!;
    public DateRange? EntryRegistrationDates { get; set; } = null!;
    public List<BrewCompUser> Entrants { get; set; } = new();
    public List<string> CategoryIds { get; set; } = new();
    public List<CompetitionEntry> Entries { get; set; } = new();
    public List<CivicAddress> DropOffAddresses { get; set; } = new();
    public List<Sponsor> Sponsors { get; set; } = new();
    public bool BJCPSanctioned { get; set; } = false;
    /// <summary>
    /// Holds a mapping of the <c cref="CompetitionEntry.EntryId">EntryId</c> of a <c cref="CompetitionEntry">CompetitionEntry</c>
    /// to the Judging ID that will be filled in on the judging sheets. These judging numbers are used to ensure anonyminity of the
    /// entries and create unbiased judging. Judging numbers will be controlled by the Competition, and can be regenerated as needed.
    /// </summary>
    public Dictionary<uint, string> JudgingIds { get; set; } = new();
}

