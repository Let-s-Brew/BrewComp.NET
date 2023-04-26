using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BrewComp.Data;

public record HomebrewClub
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; init; }
    public string? Abbreviation { get; init; }
    public Uri? Homepage { get; init; }

    public HomebrewClub(string name, string? abbreviation = null, Uri? homepage = null)
    {
        Name = name;
        Abbreviation = abbreviation;
        Homepage = homepage;
    }

    public override string ToString()
    {
        return Abbreviation == null ? Name : $"{Name} ({Abbreviation})";
    }

    public static void PopulateDb(BrewCompDbContext context, ILogger logger)
    {
        try
        {
            using (JsonDocument doc = JsonDocument.Parse(Assembly.GetExecutingAssembly().GetManifestResourceStream($"BrewComp.Data.clubs.json")))
            {
                Regex rx = new Regex(@"(?<club>[^\(]+)\s*\(*(?<abbr>[^\)]+)*\)*");
                foreach(var club in doc.RootElement.GetProperty("records").EnumerateArray()) { 
                    var nameStr = club.GetProperty("Name").GetString();
                    Uri.TryCreate(club.GetProperty("Website").GetString(), new UriCreationOptions(), out var url);
                    var match = rx.Match(nameStr);
                    if( match.Success )
                    {
                        context.Clubs.Add(new HomebrewClub(match.Groups["club"].Value, match.Groups["abbr"].Value, url));
                    } else
                    {
                        context.Clubs.Add(new HomebrewClub(nameStr));
                    }
                    logger.LogInformation($"Added Club: {nameStr}");
                }
            }
            context.SaveChanges();
        } catch (Exception ex) 
        {
            logger.LogError($"Unable to populate club database: {ex.Message}");
        }
    }
}