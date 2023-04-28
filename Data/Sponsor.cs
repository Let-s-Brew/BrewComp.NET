using System.ComponentModel.DataAnnotations.Schema;

namespace BrewComp.Data;

public class Sponsor
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImgPath { get; set; }
}
