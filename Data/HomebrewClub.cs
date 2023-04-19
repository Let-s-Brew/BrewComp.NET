using System.ComponentModel.DataAnnotations.Schema;

namespace BrewComp.Data;

public record HomebrewClub
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; init; }
    public string? Abbreviation { get; init; }
    public Uri? Homepage { get; init; }

    public HomebrewClub(string name, string abbreviation = "", Uri? homepage = null)
    {
        Name = name;
        Abbreviation = abbreviation;
        Homepage = homepage;
    }
}