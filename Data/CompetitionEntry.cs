using BrewCode.BrewGuide;
using BrewComp.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewComp.Data;

public class CompetitionEntry
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Competition Competition { get; set; } = null!;
    public uint EntryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public BrewCompUser Brewer { get; set; } = null!;
    public string? CoBrewer { get; set; }
    public IStyle Style { get; set; } = null!;
    public string? BrewersSpecs { get; set; }
    public string? Allergens { get; set; }
    public float? CompositeScore { get; set; }

}
