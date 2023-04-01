using BrewComp.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewComp.Data;

public class CompetitionEntry
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public string Name { get; set; }
    public UserIdentity Brewer { get; set; }
    public string? CoBrewer { get; set; }
    //TODO - add Style Guidelines library, and then also check if style has additional required info
    // public IStyle Style { get; set; }
    public string? BrewersSpecs { get; set; }
    public string? Allergens { get; set; }

    public CompetitionEntry(string name, UserIdentity brewer, string? coBrewer = null, string? brewersSpecs = null, string? allergens = null)
    {
        Name = name;
        Brewer = brewer;
        CoBrewer = coBrewer;
        BrewersSpecs = brewersSpecs;
        Allergens = allergens;
    }
}
