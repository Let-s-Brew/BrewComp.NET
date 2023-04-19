using BrewComp.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewComp.Data;

public class CompetitionEntry
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Competition Competition { get; set; }
    public uint EntryId { get; set; } = 1;
    [Required]
    public string Name { get; set; }
    [Required]
    public BrewCompUser Brewer { get; set; }
    public string? CoBrewer { get; set; }
    //TODO - add Style Guidelines library, and then also check if style has additional required info
    // public IStyle Style { get; set; }
    public string? BrewersSpecs { get; set; }
    public string? Allergens { get; set; }
    public float? CompositeScore { get; set; }

}
