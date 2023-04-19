using System.ComponentModel.DataAnnotations.Schema;

namespace BrewComp.Areas.Configuration.Models;

public class OAuthLoginConfig
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}
